using Microsoft.Win32;

WebView2Runtime webView2Runtime = WebView2Runtime.Current;

if (webView2Runtime is not null)
{
    Console.WriteLine("Checkning WebView2...");
    Console.WriteLine($"{webView2Runtime.Name} {webView2Runtime.Version} is installed at {webView2Runtime.Location}.");
    Console.WriteLine($"To uninstall it execute: {webView2Runtime.SilentUninstallCommand}");
}

public class WebView2Runtime
{
    string _version;
    public string Version { get { return _version; } }

    string _location;
    public string Location { get { return _location; } }

    string _name;
    public string Name { get { return _name; } }

    string _silentUninstallCommand;
    public string SilentUninstallCommand { get { return _silentUninstallCommand; } }

    bool isWebView2Initialized = false;

    static WebView2Runtime _webView2Runtime;
    public static WebView2Runtime Current
    {
        get
        {
            if (_webView2Runtime is null)
            {
                _webView2Runtime = new WebView2Runtime();
                _webView2Runtime.Initialize();
            }
            return _webView2Runtime;
        }
    }

    bool Initialize()
    {
        const string stringKeyPerLocalMachine = "Software\\Wow6432Node\\Microsoft\\EdgeUpdate\\Clients\\{F3017226-FE2A-4295-8BDF-00C3A9A7E4C5}";
        const string stringKeyPerCurrentUser = "Software\\Microsoft\\EdgeUpdate\\Clients\\{F3017226-FE2A-4295-8BDF-00C3A9A7E4C5}";

        bool isWebView2Installed = false;

        RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(stringKeyPerLocalMachine);
        if (registryKey is null)
        {
            registryKey = Registry.CurrentUser.OpenSubKey(stringKeyPerCurrentUser);
        }

        if (registryKey is not null)
        {
            const string versionWebView2 = "pv";
            const string nameWebView2 = "name";
            const string locationWebView2 = "location";
            const string silentUninstallWebView2 = "SilentUninstall";

            object versionREG_SZ;
            object nameREG_SZ;
            object locationREG_SZ;
            object silentREG_SZ;

            versionREG_SZ = registryKey.GetValue(versionWebView2);
            nameREG_SZ = registryKey.GetValue(nameWebView2);
            locationREG_SZ = registryKey.GetValue(locationWebView2);
            silentREG_SZ = registryKey.GetValue(silentUninstallWebView2);

            _version = versionREG_SZ as string;
            _name = nameREG_SZ as string;
            _location = locationREG_SZ as string;
            _silentUninstallCommand = silentREG_SZ as string;

            isWebView2Installed = true;
        }
        return isWebView2Installed;
    }
}
