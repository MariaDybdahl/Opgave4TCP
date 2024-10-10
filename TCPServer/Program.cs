using System.Net.Sockets;
using System.Net;
using System.IO;
Console.WriteLine("TCP Server:");

 // Start serveren på port 5000
        TcpListener server = new TcpListener(IPAddress.Any, 5000);
        server.Start();
        Console.WriteLine("Serveren venter på forbindelse...");

while (true)
{
            
//Acceptér forbindelse fra klienten
TcpClient client = server.AcceptTcpClient();
Console.WriteLine("Klient forbundet!");

//Starter en ny asynkron opgave for at håndtere klienten i baggrunden
Task.Run(() => HandleClient(client));

}
    
    void HandleClient(TcpClient tcpClient)
    {
        TcpClient client = tcpClient;

      
        // Få en stream til at sende og modtage beskeder
        NetworkStream stream = client.GetStream();
        StreamReader reader = new StreamReader(stream);
        StreamWriter writer = new StreamWriter(stream) { AutoFlush = true };
    //Jeg har sat AutoFlush = true isted for at skrive writer.Flush efter værre gang vi laver en writer.WriteLine();


    try {
        while (true)
        {
            //Første bedsked som bliver sent over til klient siden
            string messagekommando = "Skriv kommando ('random', 'add', 'subtract' eller 'exit')";

            //udskriver til klient (writer.WriteLine) og server konsole (Console.WriteLine)
            writer.WriteLine(messagekommando);
            Console.WriteLine("Sendt til klient:" + messagekommando);

            // Modtager kommando fra klienten, sætter den til lowercase 
            string message = reader.ReadLine()?.ToLower();
            Console.WriteLine($"Modtaget kommando fra klient: {message}");

            //hvis kommando er random
            if (message == "random")
            {
                //udskriver til klient (writer.WriteLine) og server konsole (Console.WriteLine)
                writer.WriteLine("Indtast to tal adskilt af mellemrum (f.eks. 1 10)");
                Console.WriteLine("Sendt til klient: Indtast to tal adskilt af mellemrum (f.eks. 1 10)");

                // Læser en linje fra input og splitter den op i dele baseret på mellemrum gemmes i parts af array af string.
                string[] messageParts = reader.ReadLine().Split(' ');

                //Ser om der er 2 ting i parts hvis der er
                //vil den konverterede til et helt tal som blive gemt i num1 derefter vil den konverterede det andet tal til et heltal i num2 
                if (messageParts.Length == 2 && int.TryParse(messageParts[0], out int num1ForRandom) && int.TryParse(messageParts[1], out int num2ForRandom))
                {
                    //bruger type random og laver et nyt obejkt vedd navn random
                    Random random = new Random();

                    //får nummet via random.Next som er num1, num2+1 får at for det nummer2 med. fx 1,2 vil begge værre en del af det, hvis kun uden num2+1 vil man kun havde num1 og num2 vil slet ikke ingå
                    int randomNumber = random.Next(num1ForRandom, (num2ForRandom + 1));

                    //udskriver til klient (writer.WriteLine) og server konsole (Console.WriteLine)
                    Console.WriteLine($"Random nummer mellem {num1ForRandom} og {num2ForRandom}: {randomNumber}");
                    writer.WriteLine($"Svar fra server: Random nummer mellem {num1ForRandom} og {num2ForRandom}: {randomNumber}");

                }
                //ellers få man en genneral error
                else
                {
                    //udskriver til klient (writer.WriteLine) og server konsole (Console.WriteLine)
                    Console.WriteLine($"Ugyldig tal modtaget fra klienten");
                    writer.WriteLine("Tallen du har indtastet er ugyldig");
                }
            }
            else if (message == "add")
            {
                //udskriver til klient (writer.WriteLine) og server konsole (Console.WriteLine)
                writer.WriteLine("Indtast to tal adskilt af mellemrum (f.eks. 1 10)");
                Console.WriteLine("Sendt til klient: Indtast to tal adskilt af mellemrum (f.eks. 1 10)");

                // Læser en linje fra input og splitter den op i dele baseret på mellemrum
                string[] messageParts = reader.ReadLine().Split(' ');
                //Ser om der er 2 ting i parts hvis der er
                //vil den konverterede til et helt tal som blive gemt i num1 derefter vil den konverterede det andet tal til et heltal i num2 
                if (messageParts.Length == 2 && int.TryParse(messageParts[0], out int num1) && int.TryParse(messageParts[1], out int num2))
                {
                    //result er num1 + nummer2
                    int result = num1 + num2;

                    //udskriver til klient (writer.WriteLine) og server konsole (Console.WriteLine)
                    Console.WriteLine($"Resultat af add: {result}");
                    writer.WriteLine($"Svar fra server: Resultat af add: {result}");
                }
                else
                {
                    //udskriver til klient (writer.WriteLine) og server konsole (Console.WriteLine)
                    Console.WriteLine($"Ugyldig tal modtaget fra klienten: {message}");
                    writer.WriteLine("Tallen du har indtastet er ugyldig");
                }



            }
            else if (message == "subtract")
            {
                //udskriver til klient (writer.WriteLine) og server konsole (Console.WriteLine)
                writer.WriteLine("Indtast to tal adskilt af mellemrum (f.eks. 1 10)");
                Console.WriteLine("Sendt til klient: Indtast to tal adskilt af mellemrum (f.eks. 10 1)");

                // Læser en linje fra input og splitter den op i dele baseret på mellemrum
                string[] messageParts = reader.ReadLine().Split(' ');

                //Ser om der er 2 ting i parts hvis der er
                //vil den konverterede til et helt tal som blive gemt i num1 derefter vil den konverterede det andet tal til et heltal i num2 
                if (messageParts.Length == 2 && int.TryParse(messageParts[0], out int num1) && int.TryParse(messageParts[1], out int num2))
                {
                    //result trækker num2 fra num1
                    int result = num1 - num2;
                    //udskriver til klient (writer.WriteLine) og server konsole (Console.WriteLine)
                    Console.WriteLine($"Sendt til klient: Resultat af subtract {result}");
                    writer.WriteLine($"Svar fra server: Resultat af subtract: {result}");
                }
                else
                {
                    //udskriver til klient (writer.WriteLine) og server konsole (Console.WriteLine)
                    Console.WriteLine($"Ugyldig tal modtaget fra klienten: {message}");
                    writer.WriteLine("Tallen du har indtastet er ugyldig");
                }
            }

            else if (message == "exit")
            {
                //udskriver til klient (writer.WriteLine) og server konsole (Console.WriteLine)
                Console.WriteLine("Klienten har afsluttet forbindelsen");
                writer.WriteLine("Farvel!");

                //lukker klient.
                client.Close();

                //Stopper while-løkken
                break;
            }
            else
            {
                //udskriver til klient (writer.WriteLine) og server konsole (Console.WriteLine)
                Console.WriteLine($"Ugyldig kommando modtaget fra klienten: {message}");
                writer.WriteLine("Kommandoen du har indtastet er ugyldig");
            }
        }
    }
    //Hvis der sker nogle fejl så catch fx hvis man lukker oppe i kryset ud af skriver exit
    catch (Exception ex) 
    {
        Console.WriteLine("Fejl: " + ex.Message); 
    }
}

