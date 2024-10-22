# Support for PEM encoded EC keys in .NET-Framework using BouncyCastle

Generation of an EC key pair, export and import of PEM encoded private (in PKCS#8 and SEC#1 format) and public keys (in SPKI format), key agreement and signing/verifying using the BouncyCastle classes. 

Tested with .NET Framework 4.8 and Portable.BouncyCastle 1.9.0

Sample output:

```none
-----BEGIN PRIVATE KEY-----
MIGNAgEAMBAGByqGSM49AgEGBSuBBAAKBHYwdAIBAQQgf5mXeWo4JR6vhjbQ+3Wf
2E2YkHy9boCelyPxOzHTF2egBwYFK4EEAAqhRANCAATziQ2jwjk1NdGInNBGjj+E
g4dRniDZwhPOP+t2BYX51Zun/B+0YgQJBmtVOH7wOSC66neATJtsvFQ/QaHNfZIN
-----END PRIVATE KEY-----

-----BEGIN EC PRIVATE KEY-----
MHQCAQEEIH+Zl3lqOCUer4Y20Pt1n9hNmJB8vW6Anpcj8Tsx0xdnoAcGBSuBBAAK
oUQDQgAE84kNo8I5NTXRiJzQRo4/hIOHUZ4g2cITzj/rdgWF+dWbp/wftGIECQZr
VTh+8Dkguup3gEybbLxUP0GhzX2SDQ==
-----END EC PRIVATE KEY-----

-----BEGIN PUBLIC KEY-----
MFYwEAYHKoZIzj0CAQYFK4EEAAoDQgAE84kNo8I5NTXRiJzQRo4/hIOHUZ4g2cIT
zj/rdgWF+dWbp/wftGIECQZrVTh+8Dkguup3gEybbLxUP0GhzX2SDQ==
-----END PUBLIC KEY-----

2a88c0b63634e4cfc26b5ef1c739c76358a606f86c2f55745e19bcd3035147e6
2a88c0b63634e4cfc26b5ef1c739c76358a606f86c2f55745e19bcd3035147e6

59842334e24eb9a6161ca6727bce3a89cf30bb13958d2703a6c9af8db04d3d6443af79cef6fce85b60a64fb7fd87900a018b573739af459cb1b628ec0ea0af52
True

304502205419149093424b0ee7f75788fae8a9e2cd7cb52617a24ead3dda25300db44063022100a29eed232eeba64908b932ad5d156160bb253c2fc755ae82d77acea8898c3927
True
```
