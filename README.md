# LinkRedirector

��������� ������� ��� ��������������� ������ � ��������� ���������� ��������.

# ���������

1. ������� ����� �� ������ https://disk.yandex.ru/d/gh2aIh1R8tofXA ��� ������� �� ����������
2. ����������� ���������� ������ � ����� ����������� ���������� �������, � ��� ������ �������� "C:\Program Files (x86)\Link Redirector"
3. ��������� setup.bat, �� ������� ���������� ����� ������� � ������� �����, � ������� ���������, � ���������� ��������� Path (uninstall.bat ���������� �� ��� ���������)
4. �������� ����������� ������������ � ����� settings.json (�� ���� ����)

# ���������

������������ ������� �� ���� ������. ������ ��� ��������� ����������. �������� ������ ����� ���� �������������� � ��������� �������.

������:
"Default": {
	"TargetProgram": "C:\\Program Files\\Mozilla Firefox\\firefox.exe",
	"Arguments": "-osint -url \"{0}\""
}
��� ���� ����� ����� ����� � ������� Windows:
"Default": {
    "RegistryClass": "FirefoxURL-308046B0AF4A39CB"
}
������ � ������� ����� ���������� � HKEY_CLASSES_ROOT.

������ ���� - ��� ������ �������� � ������������� ������ Pattern, Replacement � ����� �� ���� ����� TargetProgram ��� RegistryClass. ������������ ������ ��������� ������.
������:
"Redirects": [
	{
		"Pattern": "^.*?spotify\\.com\\/([^/]+)\\/([^?]*)\\?.*$",
		"Replacement": "--protocol-uri=\"spotify:$1:$2\"",
		"TargetProgram": "C:\\Users\\ZzZvAD\\AppData\\Roaming\\Spotify\\Spotify.exe"
	},
    {
      "ProcessNamePattern": "slack",
      "Pattern": "^.*$",
      "Replacement": "--single-argument $0",
      "TargetProgram": "C:\\Program Files (x86)\\Google\\Chrome\\Application\\chrome.exe"
    }
]
��� ���� ����� ����� ����� � ������� Windows:
"Redirects": [
    {
      "Pattern": "^.*?spotify\\.com\\/([^/]+)\\/([^?]*)\\?.*$",
      "Replacement": "spotify:$1:$2",
	  "RegistryClass":  "spotify"
    },
    {
      "ProcessNamePattern": "slack",
      "Pattern": "^.*$",
      "Replacement": "$0",
	  "RegistryClass": "ChromeHTML"
    }
]

� ���������� ���������:
{
	"Default": {
		"TargetProgram": "C:\\Program Files\\Mozilla Firefox\\firefox.exe",
		"Arguments": "-osint -url \"{0}\""
	},
	"Redirects": [
		{
			"Pattern": "^.*?spotify\\.com\\/([^/]+)\\/([^?]*)\\?.*$",
			"Replacement": "--protocol-uri=\"spotify:$1:$2\"",
			"TargetProgram": "C:\\Users\\ZzZvAD\\AppData\\Roaming\\Spotify\\Spotify.exe"
		},
		{
		  "ProcessNamePattern": "slack",
		  "Pattern": "^.*$",
		  "Replacement": "--single-argument $0",
		  "TargetProgram": "C:\\Program Files (x86)\\Google\\Chrome\\Application\\chrome.exe"
		}
	]
}
��� ���� ����� ����� ����� � ������� Windows:
{
  "Default": {
    "RegistryClass": "FirefoxURL-308046B0AF4A39CB"
  },
  "Redirects": [
    {
      "Pattern": "^.*?spotify\\.com\\/([^/]+)\\/([^?]*)\\?.*$",
      "Replacement": "spotify:$1:$2",
	  "RegistryClass":  "spotify"
    },
    {
      "ProcessNamePattern": "slack",
      "Pattern": "^.*$",
      "Replacement": "$0",
	  "RegistryClass": "ChromeHTML"
    }
  ]
}

# �������� �����:
1. ProcessNamePattern - ���������� ��������� ��� �������� ������������ ������������� �������� (��������, ������� ����������� �������� ������). ������������ �����������. � ������ ������������ ������������. ���� ������������ ������� ���������� �� ������� ����� ����� "#unknown#".
2. Pattern - ���������� ��������� ��� �������� ������. � ������ ������������ ������������. ������ ������������ � Replacement.
3. Replacement - ��������� ������������ �������� �� TargetProgram ��� ��������, ������� ����� ������� "%1" �� ������� ������� RegistryClass.
4. TargetProgram - ������ ���� � ����������.
5. RegistryClass - ������������ ������ �� ������� Windows �� ���� HKEY_CLASSES_ROOT.

����� ������� ��� ��������� ������������:
{
    "Default": {
        "TargetProgram": "C:\\Program Files\\Mozilla Firefox\\firefox.exe",
        "Arguments": "-osint -url \"{0}\""
    },
    "Redirects": [{
            "Pattern": "^.*?spotify\\.com\\/([^/]+)\\/([^?]*)\\?.*$",
            "Replacement": "--protocol-uri=\"spotify:$1:$2\"",
            "TargetProgram": "C:\\Users\\ZzZvAD\\AppData\\Roaming\\Spotify\\Spotify.exe"
        }, {
            "Pattern": "^.*?teams\\.microsoft\\.com(.*)$",
            "Replacement": "\"msteams:$1&anon=true&deeplinkId=e6412edf-030d-4adb-afe2-5c11d16c686b&launchAgent=join_launcher&type=meetup-join&directDl=true&msLaunch=true&enableMobilePage=true&fqdn=teams.microsoft.com\"",
            "TargetProgram": "C:\\Users\\ZzZvAD\\AppData\\Local\\Microsoft\\Teams\\current\\Teams.exe"
        }, {
			"Pattern": "^.*\\/(.*\\.zoom\\.us)\\/j\\/(\\d+).*$",
			"Replacement": "\"--url=zoommtg://$1/join?action=join&confno=$2\"",
			"TargetProgram": "C:\\Users\\ZzZvAD\\AppData\\Roaming\\Zoom\\bin\\Zoom.exe"
		}, {
            "ProcessNamePattern": "slack",
            "Pattern": "^.*$",
            "Replacement": "//b RemoteRedirect.vbs \"$0\" \"#unknown#\"",
            "TargetProgram": "C:\\Windows\\System32\\wscript.exe"
        }
    ]
}

{
  "Default": {
    "RegistryClass": "FirefoxURL-308046B0AF4A39CB"
  },
  "Redirects": [
    {
      "Pattern": "^.*?spotify\\.com\\/([^/]+)\\/([^?]*)\\?.*$",
      "Replacement": "spotify:$1:$2",
	  "RegistryClass":  "spotify"
    },
    {
      "ProcessNamePattern": "slack",
      "Pattern": "^.*$",
      "Replacement": "$0",
	  "RegistryClass": "ChromeHTML"
    }
  ]
}

{
    "Default": {
        "RegistryClass":  "FirefoxURL-308046B0AF4A39CB"
    },
    "Redirects": [{
		    "Pattern": "^.*?spotify\\.com\\/([^/]+)\\/([^?]*)\\?.*$",
		    "Replacement": "spotify:$1:$2",
		    "RegistryClass":  "spotify"
		}, {
            "Pattern": "^.*?teams\\.microsoft\\.com(.*)$",
            "Replacement": "msteams:$1&anon=true&launchAgent=join_launcher&type=meetup-join&directDl=true&msLaunch=true",
            "RegistryClass": "msteams"
        }, {
			"Pattern": "^.*\\/(.*\\.zoom\\.us)\\/j\\/(\\d+).*$",
			"Replacement": "zoommtg://$1/join?action=join&confno=$2",
			"RegistryClass": "ZoomLauncher"
		}, {
            "ProcessNamePattern": "slack",
            "Pattern": "^.*$",
            "Replacement": "//b RemoteRedirect.vbs \"$0\" \"#unknown#\"",
            "TargetProgram": "C:\\Windows\\System32\\wscript.exe"
        }
    ]
}