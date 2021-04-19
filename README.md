# LinkRedirector

Небольшая утилита для перенаправления ссылок в различные приложения напрямую.

# Установка

1. Скачать последнюю версию по ссылке https://github.com/ZzZvAD1/LinkRedirector/releases
2. Распаковать содержимое архива в папку постоянного проживания утилиты, в моём случае например "C:\Program Files (x86)\Link Redirector"
3. Запустить setup.bat, он создаст необходимы ключи реестра и добавит папку, в которой находится, в переменную окружения Path (uninstall.bat собственно всё это подчищает)
4. Добавить необходимую конфигурацию в файле settings.json (об этом ниже)

# Настройка

Конфигурация состоит из двух блоков. Первый это дефолтное приложение. Открытие ссылки через него осуществляется в последнюю очередь.

Пример:
```json
"Default": {
    "TargetProgram": "C:\\Program Files\\Mozilla Firefox\\firefox.exe",
    "Arguments": "-osint -url \"{0}\""
}
```
Или тоже самое через класс в реестре Windows:
```json
"Default": {
    "RegistryClass": "FirefoxURL-308046B0AF4A39CB"
}
```
Классы в реестре можно посмотреть в HKEY_CLASSES_ROOT.

Второй блок - это массив объектов с обязательными полями Pattern, Replacement и одним из двух полей TargetProgram или RegistryClass. Используется первый совпавший объект.
Пример:
```json
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
```
Или тоже самое через класс в реестре Windows:
```json
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
```

В результате получится:
```json
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
```
Или тоже самое через класс в реестре Windows:
```json
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
```

# Описание полей:
1. ProcessNamePattern - регулярное выражение для проверки наименования родительского процесса (процесса, который инициировал открытие ссылки). Определяется нестабильно. В случае несовпадения игнорируется. Если родительский процесс определить не удалось будет равен "#unknown#".
2. Pattern - регулярное выражение для проверки ссылки. В случае несовпадения игнорируется. Группы используются в Replacement.
3. Replacement - аргументы запускаемого процесса из TargetProgram или значение, которое будет заменой "%1" из команды запуска RegistryClass.
4. TargetProgram - полный путь к приложению.
5. RegistryClass - наименование класса из реестра Windows по пути HKEY_CLASSES_ROOT.

Далее примеры для различных конфигураций:
```json
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
            "Pattern": "^.*?teams\\.microsoft\\.com(.*)$",
            "Replacement": "\"msteams:$1&anon=true&launchAgent=join_launcher&type=meetup-join&directDl=true&msLaunch=true\"",
            "TargetProgram": "C:\\Users\\ZzZvAD\\AppData\\Local\\Microsoft\\Teams\\current\\Teams.exe"
        }, 
		{
            "Pattern": "^.*\\/(.*\\.zoom\\.us)\\/j\\/(\\d+).*$",
            "Replacement": "\"--url=zoommtg://$1/join?action=join&confno=$2\"",
            "TargetProgram": "C:\\Users\\ZzZvAD\\AppData\\Roaming\\Zoom\\bin\\Zoom.exe"
        }, 
		{
            "ProcessNamePattern": "slack",
            "Pattern": "^.*$",
            "Replacement": "//b RemoteRedirect.vbs \"$0\" \"#unknown#\"",
            "TargetProgram": "C:\\Windows\\System32\\wscript.exe"
        }
    ]
}
```
```json
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
```
```json
{
    "Default": {
        "RegistryClass":  "FirefoxURL-308046B0AF4A39CB"
    },
    "Redirects": [
	    {
            "Pattern": "^.*?spotify\\.com\\/([^/]+)\\/([^?]*)\\?.*$",
            "Replacement": "spotify:$1:$2",
            "RegistryClass":  "spotify"
        }, 
		{
            "Pattern": "^.*?teams\\.microsoft\\.com(.*)$",
            "Replacement": "msteams:$1&anon=true&launchAgent=join_launcher&type=meetup-join&directDl=true&msLaunch=true",
            "RegistryClass": "msteams"
        }, 
		{
            "Pattern": "^.*\\/(.*\\.zoom\\.us)\\/j\\/(\\d+).*$",
            "Replacement": "zoommtg://$1/join?action=join&confno=$2",
            "RegistryClass": "ZoomLauncher"
        }, 
		{
            "ProcessNamePattern": "slack",
            "Pattern": "^.*$",
            "Replacement": "//b RemoteRedirect.vbs \"$0\" \"#unknown#\"",
            "TargetProgram": "C:\\Windows\\System32\\wscript.exe"
        }
    ]
}
```