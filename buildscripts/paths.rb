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

	:repositories_nuspec => {
		:root => File.join(root_folder, "build", "nuget", Projects[:repositories][:dir]),
		:lib => File.join(root_folder, "build", "nuget", Projects[:repositories][:dir], "lib"),
		:net40 => File.join(root_folder, "build", "nuget", Projects[:repositories][:dir], "lib", "net40"),
		:net45 => File.join(root_folder, "build", "nuget", Projects[:repositories][:dir], "lib", "net45"),
		:sl4 => File.join(root_folder, "build", "nuget", Projects[:repositories][:dir], "lib", "sl40"),
		:wp71 => File.join(root_folder, "build", "nuget", Projects[:repositories][:dir], "lib", "wp71"),
		:wp8 => File.join(root_folder, "build", "nuget", Projects[:repositories][:dir], "lib", "windowsphone8"),
	},

	:qdfeed_tests => File.join(root_folder, Projects[:qdfeed][:test_dir]),

	:qdfeed_bin => 'Placeholder - set once you have environment information',
	:qdfeed_net45_bin => 'Placeholder - set once you have environment information',
	:qdfeed_wp7_bin => 'Placeholder - set once you have environment information',
	:qdfeed_wp8_bin => 'Placeholder - set once you have environment information',
	:qdfeed_sl4_bin => 'Placeholder - set once you have environment information',

	:nuget_bin => File.join(root_folder, ".nuget")
}