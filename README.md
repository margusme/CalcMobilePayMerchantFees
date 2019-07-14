This is a sample Visual Studio .net core console application solution for demostrating how to allow reading of some payment transactions from any kind of source and using any kind of format and saving results in any kind of format to any kind of destination. 
Specifically in this solution data is read from three column format text file, payment fee will be calculated for each transaction depending on merchant or the date of transaction. Results are outputted to the console.

Incoming transactions are in format 
Date merchantName amount

Outgoing transactions are in format
Date merchantName fee 

Incoming data will be read always from the file named transactions.txt. File can be located at the path where program is running or file path can be specified by command line parameter after the program name.

For example for Windows operating system please use following format:
"C:\Program Files\dotnet\dotnet.exe" CalcMobilePayMerchantFees.dll
or
"C:\Program Files\dotnet\dotnet.exe" CalcMobilePayMerchantFees.dll C:\Temp 

In the first case transactions.txt has to be in the same folder with CalcMobilePayMerchantFees.dll, in the second case it has to be in the folder C:\Temp

For running the code and debugging please download Visual Studio 2019 and open the solution CalcMobilePayMerchantFees.sln