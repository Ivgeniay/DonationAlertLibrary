![.NET 6+](https://img.shields.io/badge/.NET-6%2B-blueviolet)![](https://img.shields.io/badge/In%20development%20-blueviolet)
# Donation Allert Library 

# Centrifugo ApiV1 and WebSocket Clients Library

This library provides an easy way to connect and interact with the Donation Alerts API and Centrifugo WebSocket channels in a C# application. It supports all available ApiV1 features and channel subscriptions, real-time notification processing and activity logging.

## Features

- **Authorization**: Supports both types of authorization.
- **WebSocket Connection**: Connect to Centrifugo WebSocket server.
- **Channel Subscription**: Subscribe to multiple channels and handle messages from them.
- **Logging**: Log connection statuses and received messages.
- **Error Handling**: Capture and log errors during connection and message processing.

## Installation

Add the library to your project by including the source files or compiling it into a DLL and referencing it in your project.

[OFFICIAL DOCUMENTATION](https://www.donationalerts.com/apidoc "For better understanding, read the official documentation") 

## Usage

### 1. Authorization

You can use the 2 provided classes for authorization DonationAlertsGrandTypeAuth and DonationAlertsGrandTypeImplicit. Both classes represent the oath authorization method for obtaining an access key. 

Their difference is that DonationAlertsGrandTypeAuth is a more “heavy” version of authorization and requires the developer to be more responsible regarding the storage of user keys. This type of authorization makes it possible to update the user’s access keys until the user revokes them.
The following example will be based on this type of authorization.

```csharp
ILogger logger = new Logger().LogAction((message, logLevel) =>
{
    if (logLevel == LogLevel.Error)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"{logLevel} ({DateTime.Now}):");
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write($"{message}\n");
    }
    else if (logLevel == LogLevel.Warning)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"{logLevel} ({DateTime.Now}):");
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write($"{message}\n"); 
    }
    else
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"{logLevel} ({DateTime.Now}):");
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write($"{message}\n"); 
    }

});

Credentials credentials = new(
    clientId: 13330, 
    clientSecret: "i416YiL9yVOuCYcxI8CpM6swcZXFK5Tsj5z3i5sX", 
    redirect: "http://localhost",
    port: "3180",
    ScopeType.OauthDonationSubscribe,
    ScopeType.OauthGoalSubscribe,
    ScopeType.OauthPollSubscribe,
    ScopeType.OauthCustomAlertStore,
    ScopeType.OauthDonationIndex,
    ScopeType.OauthUserShow
    );

DonationAlertsGrandTypeAuth alertsGrandTypeAuth = new(credentials, logger);
```

### 2. Recieving 'Code' and AccessToken (using SimpleServer)

If you are using the desktop version of the application, you can use the SimpleServer class to open a simple local server and the static OpenProcess class to open the default browser on the system to redirect the user to the DonationAlerts website.

```csharp
SimpleServer simpleServer = new(credentials.Redirect, credentials.Port, logger);
simpleServer.Start();

logger?.Log("Opening browser...");
int idProcess = OpenProcess.Open(
    alertsGrandTypeAuth.GetAuthorizationUrl(),
    logger);

await Task.Run(() =>
{
    logger?.Log("Waiting for code...");
    Thread.Sleep(1500);
});

CodeModel? codeModel = await simpleServer.AwaitCode();
logger?.Log(codeModel?.ToString() ?? "null");
simpleServer.Dispose();
OpenProcess.Close(idProcess, logger);

AccessTokenResponse? accesTokenResponse = await alertsGrandTypeAuth.GetAccessTokenAsync(accessTokenRequest);
```

### 3. Recieving User

To receive messages from Centrifugo, you must obtain user data.

```csharp
ApiV1 apiV1 = new(accesTokenResponse.Access_token, logger);
UserWrap? userWrap = await apiV1.GetUserProfileAsync();
```

### 4. RecieveClient, Subscription and Connect to Centrifugo channels

```csharp
centrifugoClientFacade = new(credentials, userWrap.Data, accesTokenResponse, logger);
await centrifugoClientFacade.Start();
```

### 5. Subscribe to Centrifugo events

```csharp
centrifugoClientFacade.OnDonationReceived += (donation) => Console.WriteLine(donation);
centrifugoClientFacade.OnGoalLaunchUpdate += (goalInfo) => Console.WriteLine(goalInfo);
centrifugoClientFacade.OnGoalUpdated += (goalsUpdateWrapper) => Console.WriteLine(goalsUpdateWrapper);
centrifugoClientFacade.OnPollUpdated += (pollDataWrapper) => Console.WriteLine(pollDataWrapper);
centrifugoClientFacade.OnMessageReceived += (message) => Console.WriteLine(message);
```



## License:
This library is distributed under the MIT license. See the LICENSE file for details. 

This example README.md should give a good understanding of how to use your library and what features it provides. You can adapt it to your needs and add additional sections if necessary.