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

    static WebView2Runtime? _webView2Runtime;
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

    private WebView2Runtime()
    {
        _version = _name = _silentUninstallCommand = _location = string.Empty;
    }

    void Initialize()
    {
        const string stringKeyPerLocalMachine = "Software\\Wow6432Node\\Microsoft\\EdgeUpdate\\Clients\\{F3017226-FE2A-4295-8BDF-00C3A9A7E4C5}";
        const string stringKeyPerCurrentUser = "Software\\Microsoft\\EdgeUpdate\\Clients\\{F3017226-FE2A-4295-8BDF-00C3A9A7E4C5}";

        //When WebView2 is installed, it should be registered on one of two reg keys
        RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(stringKeyPerLocalMachine);
        if (registryKey is null)
        {
            registryKey = Registry.CurrentUser.OpenSubKey(stringKeyPerCurrentUser);
        }

        if (registryKey is not null)
        {
            _version = GetValue(registryKey, "pv");
            _name = GetValue(registryKey, "name");
            _location = GetValue(registryKey, "location");
            _silentUninstallCommand = GetValue(registryKey, "SilentUninstall");
        }
    }

    private string GetValue(RegistryKey registryKey, string valueKey)
    {
        string? value = registryKey.GetValue(valueKey) as string;
        return value is null ? string.Empty : value;
    }
}
