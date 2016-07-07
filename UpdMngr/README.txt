The aim is to implement an autonomous DSS as defined in [2] with upstream server being
one of Microsoft's ones.
We consider implementing :
- invoking the Server Sync Web Service hosted on Microsoft update.
- invoking the DSS Authorization Web Service hosted on Microsoft update.

As of now we still have to understand what kind of DSS authorization Microsoft upstream
server implements.

As of writing, the Microsoft update service is located at http://update.microsoft.com

References
==========

[1] [MS-WUSP]: Windows Update Services: Client-Server Protocol
https://msdn.microsoft.com/en-us/library/cc251937.aspx
[2] [MS-WSUSSS]: Windows Update Services: Server-Server Protocol
https://msdn.microsoft.com/en-us/library/dd305101.aspx
[3] [MS-DRSR]: Directory Replication Service (DRS ... - MSDN - Microsoft
https://msdn.microsoft.com/en-us/library/cc228086.aspx