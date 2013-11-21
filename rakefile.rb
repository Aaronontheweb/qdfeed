$: << './'
require "rubygems"
require "bundler"
Bundler.setup

require 'albacore'
require 'version_bumper'

#-----------------------
# Local dependencies
#-----------------------
require File.expand_path(File.dirname(__FILE__)) + '/buildscripts/project_data'
require File.expand_path(File.dirname(__FILE__)) + '/buildscripts/paths'

#-----------------------
# Environment variables
#-----------------------
@env_buildconfigname = "Release"

def env_buildversion
    bumper_version.to_s
end

#-----------------------
# Control-Flow
#-----------------------

desc "Default task - builds MarkedUp and runs the test suite."
task :default => [:bump_build_number, :nunit_tests]

desc "Builds all of the nuget packages"
task :nuget => [:default, :qdfeed_pack]

#-----------------------
# Build Management
#-----------------------

msbuild :msbuild => [:assemblyinfo] do |msb|
    msb.properties :configuration => @env_buildconfigname
    msb.targets :Clean, :Build #Does the equivalent of a "Rebuild Solution"
    msb.solution = File.join(Folders[:root], Files[:solution])
    msb.other_switches :m => true
end

#-----------------------
# Version Management
#-----------------------

assemblyinfo :assemblyinfo do |asm|
    assemblyInfoPath = File.join(Folders[:root], Files[:assembly_info])

    asm.input_file = assemblyInfoPath
    asm.output_file = assemblyInfoPath

    asm.version = env_buildversion
    asm.file_version = env_buildversion
end

desc "Increments the build number for the project"
task :bump_build_number do
    bumper_version.bump_build
    bumper_version.write(File.join(Folders[:root], Files[:version]))
end

desc "Increments the revision number for the project"
task :bump_revision_number do
    bumper_version.bump_revision
    bumper_version.write(File.join(Folders[:root], Files[:version]))
end

desc "Increments the minor version number for the project"
task :bump_minor_version_number do
    bumper_version.bump_minor
    bumper_version.write(File.join(Folders[:root], Files[:version]))
end

desc "Increments the major version number for the project"
task :bump_major_version_number do
    bumper_version.bump_major
    bumper_version.write(File.join(Folders[:root], Files[:version]))
end

#-----------------------
# Testing
#-----------------------
nunit :nunit_tests => [:msbuild] do |nunit|
    nunit.command = Commands[:nunit]
    #nunit.options '/framework v4.5.30319'
    print "#{Folders[:qdfeed_tests]}/bin/#{@env_buildconfigname}/#{Files[:qdfeed_net35_bin][:test]}"
    nunit.assemblies "#{Folders[:qdfeed_tests]}/bin/#{@env_buildconfigname}/#{Files[:qdfeed_net35_bin][:test]}"
end

#-----------------------
# File Output
#-----------------------
desc "Sets the output directories according to our build environment"
task :set_output_directories do
    Folders[:qdfeed_net35_bin] = File.join(Folders[:root], Projects[:qdfeed][:dir], 'bin', @env_buildconfigname)
    Folders[:qdfeed_net45_bin] = File.join(Folders[:root], Projects[:qdfeed_net45][:dir], 'bin', @env_buildconfigname)
    Folders[:qdfeed_wp7_bin] = File.join(Folders[:root], Projects[:qdfeed_wp7][:dir], 'bin', @env_buildconfigname)
    Folders[:qdfeed_wp8_bin] = File.join(Folders[:root], Projects[:qdfeed_wp8][:dir], 'bin', @env_buildconfigname)
    Folders[:qdfeed_sl4_bin] = File.join(Folders[:root], Projects[:qdfeed_silverlight][:dir], 'bin', @env_buildconfigname)
end

desc "Wipes out the build folder so we have a clean slate to work with"
task :clean_output_directories => :set_output_directories do
    puts "Flushing build folder..."
    flush_dir(Folders[:out])
end

desc "Creates all of the output folders we need for ILMerge / NuGet"
task :create_output_directories => :set_output_directories do
    create_dir(Folders[:out])
    create_dir(Folders[:nuget_build])
    create_dir(Folders[:qdfeed_nuspec][:root])
    create_dir(Folders[:qdfeed_nuspec][:lib])
    create_dir(Folders[:qdfeed_nuspec][:net35])
    create_dir(Folders[:qdfeed_nuspec][:net45])
    create_dir(Folders[:qdfeed_nuspec][:sl4])
    create_dir(Folders[:qdfeed_nuspec][:wp71])
    create_dir(Folders[:qdfeed_nuspec][:wp8])
end

desc "Bin output for QDFeed.NET35"
output :net35_output => [:clean_output_directories, :create_output_directories] do |out|
    out.from Folders[:qdfeed_net35_bin]
    out.to Folders[:qdfeed_nuspec][:net35]
    out.file Files[:qdfeed_net35_bin][:bin]
end

desc "Bin output for QDFeed.NET45"
output :net45_output => [:clean_output_directories, :create_output_directories] do |out|
    out.from Folders[:qdfeed_net45_bin]
    out.to Folders[:qdfeed_nuspec][:net45]
    out.file Files[:qdfeed_net45_bin][:bin]
end

desc "Bin output for QDFeed.WP71"
output :wp7_output => [:clean_output_directories, :create_output_directories] do |out|
    out.from Folders[:qdfeed_wp7_bin]
    out.to Folders[:qdfeed_nuspec][:wp71]
    out.file Files[:qdfeed_wp7_bin][:bin]
end

desc "Bin output for QDFeed.WP8"
output :wp8_output => [:clean_output_directories, :create_output_directories] do |out|
    out.from Folders[:qdfeed_wp8_bin]
    out.to Folders[:qdfeed_nuspec][:wp8]
    out.file Files[:qdfeed_wp8_bin][:bin]
end

desc "Bin output for QDFeed.Silverlight"
output :silverlight_output => [:clean_output_directories, :create_output_directories] do |out|
    out.from Folders[:qdfeed_silverlight_bin]
    out.to Folders[:qdfeed_nuspec][:sl4]
    out.file Files[:qdfeed_silverlight_bin][:bin]
end

desc "All NuGet output"
task :all_output => [:net35_output, :net45_output, :wp7_output,
                    :wp8_output, :silverlight_output]

#-----------------------
# NuSpec
#-----------------------
nuspec :qdfeed_nuspec => [:all_output] do |nuspec|
    nuspec.id = Projects[:nuget_id]
    nuspec.version = env_buildversion
    nuspec.authors = Projects[:qdfeed][:authors]
    nuspec.description = Projects[:qdfeed][:description]
    nuspec.title = Projects[:qdfeed][:title]
    nuspec.language = Projects[:language]
    nuspec.licenseUrl = Projects[:licenseUrl]
    nuspec.projectUrl = Projects[:projectUrl]
    nuspec.output_file = File.join(Folders[:nuget_build], "#{Projects[:qdfeed][:id]}(#{@env_buildconfigname})-v#{env_buildversion}.nuspec")
    nuspec.tags = "rss atom rss2.0 atom1.0 syndication qdfeed qdrss feeds xml parsing net40"
end

#-----------------------
# NuGet (Packing)
#-----------------------
desc "Packs a build of the MarkedUp WinRT SDK into a NuGet package"
nugetpack :qdfeed_pack => [:qdfeed_nuspec] do |nuget|
    nuget.command = Commands[:nuget]
    nuget.nuspec = File.join(Folders[:nuget_build], "#{Projects[:qdfeed][:id]}-v#{env_nuget_version}(#{@env_buildconfigname}).nuspec")
    nuget.base_folder = File.join(Folders[:nuget_build], Folders[:qdfeed_nuspec][:root])
    nuget.output = Folders[:nuget_build]
end
