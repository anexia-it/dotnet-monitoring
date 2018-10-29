# Anexia Monitoring

Package to monitor dependency and framework versions for .NET Frameworks. It can be also used to check if the website is alive and working correctly.

## Installation and configuration

Install the package via NuGet: "VersionMonitorNet"

Set Access Token and register monitoring routes before adding the default routes in Global.asax.cs:

		...        
		protected void Application_Start()
        {
            Anexia.Monitoring.VersionMonitor.SetAccessToken("custom_access_token");
            Anexia.Monitoring.VersionMonitor.RegisterModulesInfoMonitor(RouteTable.Routes, Assembly.GetExecutingAssembly());
            Anexia.Monitoring.VersionMonitor.RegisterServiceStateMonitor(RouteTable.Routes);
            ...
        }
		...

## Usage

The package registers some custom REST endpoints which can be used for monitoring. Make sure that the `custom_access_token` is defined, since this is used for authorization.

#### Version monitoring of core and composer packages

Returns all a list with dependency and framework version information.

**URL:** `/anxapi/v1/modules?access_token=custom_access_token`

Response headers:

	Status Code: 200 OK
	Access-Control-Allow-Credentials: true
	Access-Control-Allow-Methods: GET, OPTIONS
	Access-Control-Allow-Origin: *
	Content-Type: application/json; charset=utf-8

Response body:

	"runtime":{
		"platform":"dotnet",
		"platform_version":"4.5.2",
		"framework":".NETFramework",
		"framework_installed_version":"4.5.2",
		"framework_newest_version":".NETFramework"
	},
	"modules":[{
			"name":"AutoMapper",
			"installed_version":"4.0.4.0",
			"newest_version":"7.0.1",
			"licence_urls": ["https://github.com/AutoMapper/AutoMapper/blob/master/LICENSE.txt"]
		},
		{
			"name": "CsvHelper",
			"installed_version": "7.0.0.0",
			"newest_version": "7.1.1",
			"licence_urls": ["https://raw.githubusercontent.com/JoshClose/CsvHelper/master/LICENSE.txt"]
		},
		...
	]}

#### Live monitoring

This endpoint can be used to verify if the application is alive and working correctly.

**URL:** `/anxapi/v1/up?access_token=custom_access_token`

Response headers:

	Status Code: 200 OK
	Access-Control-Allow-Credentials: true
	Access-Control-Allow-Methods: GET, OPTIONS
	Access-Control-Allow-Origin: *
	Content-Type: text/plain; charset=utf-8

Response body:

	OK

## List of developers

* Susanne Meier <SMeier@anexia-it.com>
* Joachim Eckerl <JEckerl@anexia-it.com>

## Project related external resources

* [.NET Documentation](https://docs.microsoft.com/en-us/dotnet/)
