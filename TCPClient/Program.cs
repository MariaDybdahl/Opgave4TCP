using System;
using System.Net;
using System.Net.Sockets;
using System.Text;


Console.WriteLine("TCP Client:");

// Opret forbindelse til serveren på localhost (127.0.0.1) og port 5000
TcpClient client = new TcpClient("127.0.0.1", 5000);
Console.WriteLine("Forbundet til serveren!");

// Få en stream til at sende og modtage beskeder
NetworkStream stream = client.GetStream();
StreamReader reader = new StreamReader(stream);
StreamWriter writer = new StreamWriter(stream) { AutoFlush = true }; 
//Jeg har sat AutoFlush = true i sted for at skrive writer.Flush efter værre gang vi laver en writer.WriteLine();

// Klienten skal fortsætte med at sende kommandoer
try { 
while (true)
{
    //Læser første bedsked
    string FirstMessage = reader.ReadLine();
    
    //Skriver bedsked ud i konsolen
    Console.WriteLine($"Svar fra server: {FirstMessage}");
        
    //Skriver et input
    string input = Console.ReadLine();

    // Send kommandoen til serveren
    writer.WriteLine(input);

    // Læs svaret fra serveren
    string response = reader.ReadLine();
    Console.WriteLine($"Svar fra server: {response}");

    // Hvis serveren sender en besked om at lukke, så afbryd
    if (input == "exit")
    {
        break;
    }

    // Hvis serveren beder om input til tal, skal klienten svare
    if (response.Contains("Indtast to tal"))
    {
        //Kan skrive tal som et input
        string numberInput = Console.ReadLine();
        writer.WriteLine(numberInput);  // Send nummerinput tilbage til serveren
        Console.WriteLine("Sendt til server: " + numberInput); //skriver nummer ud til konsole på klient side
        string responseForTwo = reader.ReadLine(); //Læser den bedsked man fået fra server af response
            Console.WriteLine(responseForTwo); //udskriver bedsked i konsole.
        
       
    }
  
}
}
//Hvis der sker nogle fejl så catch fx hvis man lukker oppe i kryset ud af skriver exit
catch (Exception ex)
{ 
    Console.WriteLine("Fejl: " + ex.Message); 
}



