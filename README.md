# Support for PEM encoded EC keys in .NET-Framework using BouncyCastle

Generation of an EC key pair, export and import of PEM encoded private (in PKCS#8 and SEC#1 format) and public keys (in SPKI format), key agreement and signing/verifying using the BouncyCastle classes. 

Tested with .NET Framework 4.8 and Portable.BouncyCastle 1.9.0

Sample output:

```none
-----BEGIN PRIVATE KEY-----
MIGNAgEAMBAGByqGSM49AgEGBSuBBAAKBHYwdAIBAQQgFvW4lg6NpjzP8ov1r28x
fy7zwHaenrwbWhvIKYIJQuqgBwYFK4EEAAqhRANCAAQRJ4t9t5bp1dDv3Fjhfxt7
eHsXJB1yoXWaFuymgApJZyX/AhQ7lSGmogOMwu+ySwXvWCDikn+GYQz3bkqbF6Qa
-----END PRIVATE KEY-----

-----BEGIN EC PRIVATE KEY-----
MHQCAQEEIBb1uJYOjaY8z/KL9a9vMX8u88B2np68G1obyCmCCULqoAcGBSuBBAAK
oUQDQgAEESeLfbeW6dXQ79xY4X8be3h7FyQdcqF1mhbspoAKSWcl/wIUO5UhpqID
jMLvsksF71gg4pJ/hmEM925KmxekGg==
-----END EC PRIVATE KEY-----

-----BEGIN PUBLIC KEY-----
MFYwEAYHKoZIzj0CAQYFK4EEAAoDQgAEESeLfbeW6dXQ79xY4X8be3h7FyQdcqF1
mhbspoAKSWcl/wIUO5UhpqIDjMLvsksF71gg4pJ/hmEM925KmxekGg==
-----END PUBLIC KEY-----

-----BEGIN PUBLIC KEY-----
MFYwEAYHKoZIzj0CAQYFK4EEAAoDQgAEESeLfbeW6dXQ79xY4X8be3h7FyQdcqF1
mhbspoAKSWcl/wIUO5UhpqIDjMLvsksF71gg4pJ/hmEM925KmxekGg==
-----END PUBLIC KEY-----

a740b4fc54f9503eeecb552aa2fa93b0dc03069228ef28a40c61083d6d465f57
a740b4fc54f9503eeecb552aa2fa93b0dc03069228ef28a40c61083d6d465f57

66821af237c328f2a547c4222781d2a0ea64e8d7457bc66b6b5f36f6e238e4766f8656cc8395feffac89678dfd61306a4b3a8118e66ce86fba9045c8b0036ec1
True

304502202707478c515811a66efcb88952327516592a723618957ff96fc09eb3406db04a022100e4ebdc605f40596cbb17d8f9b6509e4ccfadb42a00dfd4a24feebedc0613aa98
True
```
