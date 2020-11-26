# QuickUnity

QuickUnity is a library that makes the basic operations of unity simple and easy.

# Networking

```c#
NTPResponse ntpResponse = await NTP.GetAsync();
Debug.Log(ntpResponse.Now);

QuickResponse quickResponse = await QuickRequest.GetAsyncAsQuickResponse("https://www.google.com/");
Debug.Log(quickResponse.GetText());
```
