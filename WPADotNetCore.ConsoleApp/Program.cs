// See https://aka.ms/new-console-template for more information
using Microsoft.Identity.Client;
using WPADotNetCore.ConsoleApp;

Console.WriteLine("Hello, World!");
//AdoDotNetExample adoDotNetExample = new AdoDotNetExample();
////adoDotNetExample.Delete();
//DapperExample dapperExample = new DapperExample();
////dapperExample.Read();
////dapperExample.Create("arar", "ddd", "rtrt");
////dapperExample.Edit(2);
////dapperExample.Delete(2);
//EFCoreExample eFCore=new EFCoreExample();
////eFCore.Create("AA","BB","CC");
////eFCore.Update(2, "ATT", "DTT", "CTT");
//eFCore.Edit(2);
////eFCore.Read();

//AdoDotNetExampleWithService adoDotNetExample= new AdoDotNetExampleWithService();
//adoDotNetExample.Read();
//adoDotNetExample.Edit();
//adoDotNetExample.Create();
//adoDotNetExample.Delete();

DapperExampleWithService dapperExample=new DapperExampleWithService();
dapperExample.Read();
dapperExample.Edit(3);
dapperExample.Create("Thet Paing Phyoe","About Yangon","Yangon is the biggest city in Myanmr");
dapperExample.Delete(5);