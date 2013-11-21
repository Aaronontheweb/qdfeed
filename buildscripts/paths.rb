#----------------------------------
# Environment variables for Quick and Dirty Feed Parser
#----------------------------------

root_folder = File.expand_path("#{File.dirname(__FILE__)}/..")


Folders={
	:root => root_folder,
	:tools => "tools",
	:out => "build",
	:nunit => File.join(root_folder, "tools", "nunit"),

	:nuget_build => File.join(root_folder, "build", "nuget"),

	:qdfeed_nuspec => {
		:root => File.join(root_folder, "build", "nuget", Projects[:qdfeed][:dir]),
		:lib => File.join(root_folder, "build", "nuget", Projects[:qdfeed][:dir], "lib"),
		:net35 => File.join(root_folder, "build", "nuget", Projects[:qdfeed][:dir], "lib", "net35"),
		:net45 => File.join(root_folder, "build", "nuget", Projects[:qdfeed][:dir], "lib", "net45"),
		:sl4 => File.join(root_folder, "build", "nuget", Projects[:qdfeed][:dir], "lib", "sl40"),
		:wp71 => File.join(root_folder, "build", "nuget", Projects[:qdfeed][:dir], "lib", "wp71"),
		:wp8 => File.join(root_folder, "build", "nuget", Projects[:qdfeed][:dir], "lib", "windowsphone8"),
	},

	:qdfeed_tests => File.join(root_folder, Projects[:qdfeed][:test_dir]),

	:qdfeed_net35_bin => 'Placeholder - set once you have environment information',
	:qdfeed_net45_bin => 'Placeholder - set once you have environment information',
	:qdfeed_wp7_bin => 'Placeholder - set once you have environment information',
	:qdfeed_wp8_bin => 'Placeholder - set once you have environment information',
	:qdfeed_sl4_bin => 'Placeholder - set once you have environment information',

	:nuget_bin => File.join(root_folder, ".nuget")
}

Files = {
	:solution => "QDFeedParser.sln",
	:assembly_info => "SharedAssemblyInfo.cs",
	:version => "VERSION",

	:qdfeed_net35_bin => {
		:bin => "#{Projects[:qdfeed][:id]}.dll",
		:test => "#{Projects[:qdfeed][:test_dir]}.dll"
	},

	:qdfeed_net45_bin => {
		:bin => "#{Projects[:qdfeed_net45][:id]}.dll"
	},

	:qdfeed_wp7_bin => {
		:bin => "#{Projects[:qdfeed_wp7][:id]}.dll"
	},

	:qdfeed_wp8_bin => {
		:bin => "#{Projects[:qdfeed_wp8][:id]}.dll"
	},

	:qdfeed_silverlight_bin => {
		:bin => "#{Projects[:qdfeed_silverlight][:id]}.dll"
	}
}

#-----------------------
# Commands
#-----------------------
Commands = {
	:nunit => File.join(Folders[:nunit], "nunit-console.exe"),
	:nuget => File.join(Folders[:nuget_bin], "NuGet.exe")
}

#safe function for creating output directories
def create_dir(dirName)
	if !File.directory?(dirName)
		FileUtils.mkdir(dirName) #creates the /build directory
	end
end

#Deletes a directory from the tree (to keep the build folder clean)
def flush_dir(dirName)
	if File.directory?(dirName)
		FileUtils.remove_dir(dirName, true)
	end
end