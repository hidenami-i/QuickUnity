# QuickUnity

QuickUnity is a library that makes the basic operations of unity simple and easy.

## Instructions

### Package Manager

1. Install UniTask using Unity Package Manager
    - https://github.com/Cysharp/UniTask
    - https://openupm.com/packages/com.cysharp.unitask/
2. The GIT url you need to add to the Package manager is the following
    - https://github.com/hidenami-i/QuickUnity.git

## Networking

```c#
NTPResponse ntpResponse = await NTP.GetAsync();
Debug.Log(ntpResponse.Now);

QuickResponse quickResponse = await QuickRequest.GetAsyncAsQuickResponse("https://www.google.com/");
Debug.Log(quickResponse.GetText());
```
