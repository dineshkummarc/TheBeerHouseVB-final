August 9, 2006

IMPORTANT WROX SUPPORT NOTE:
As this VB version of the code for ASP.NET 2.0 Web Site Programming: Problem Design Solution (ISBN: 0-7645-8464-2) has been kindly created by a reader (please see "MythicalMe" Darren's note below) the Wrox customer service/technical support group cannot provide support for this VB code. Please continue to use the book's p2p.wrox.com forum at http://p2p.wrox.com/forum.asp?FORUM_ID=261 for questions about the VB code.

Please give Darren a big thanks for taking this on!

Jim Minatel


Hi,

You all know me as MythicalMe. Over the past few weeks I have been translating Marco Bellinaso's C# code for Wrox ASP.NET 2.0 Website Programming: Problem - Design - Solution. The code zipped up here is the translation through Chapter 9 (completed and tested now). I have unit tested most of the code, but I am only confident that the syntax errors have been eliminated.  I strongly suggest that you use the code only for instructional purposes.

Most of the code is a straight-forward translation. Some, required a little bit of rework. None of the database stored procedures has been altered. You will notice that I have rearranged the codein some of the classes. The arrangement doesn't alter the performance, it is only a preference that I have for making code somewhat more readable.

When creating the solution, I failed to note the location and thus there is a folder called "TheBeerHouseVB". It contains the solution.

BUGS:

I ran across one minor bug in the Chapter 9 code that I have not fixed, because the Marco's code has the error too! When the shopping cart wizard enters the second step an attempt is made to fill in the shipping information from the profile address. In every control it is OK to have no profile except the ddlCountry control. It will return an error because the control can't handle a 'NuLL' value. To fix this, a test of the profile's Address/Country value should be checked.

There are probably more. If you run across something please let me know. My e-mail address is dkindber@telus.net


FINAL RELEASE CODE:

The code in this release has been tested extensively and I am as confident that it is as bug free as any developer can be. I monitor the p2p.wrox.com forums and answer questions whenever possible. I will answer questions regarding this code in the books forum. You are always welcome to e-mail me at the above address, but I reserve the right to respond in my own time. Please do not make multiple requests or ask that I help you with a programming problem. It is my policy to ignore such requests.

The bug listed above has been fixed in the VB code and some minor bugs.
  

DARREN J KINDBERG
