using MqttDemo.Helpers;
using MQTTnet;

// Start the server
await MqttServerHelper.StartMqttServer();

// Setup fake data for the demo
var topics = new List<string> { "Plant", "Animal" };
var contentToPublish = new List<(string, string)>()
{
    (topics[1], "Dog"),
    (topics[0], "Carrots"),
    (topics[1], "Cat"),
    (topics[0], "Tomatos"),
};

// Start listening to our topics
foreach (var topic in topics)
{
    await DataTransmissionHelper.SubscribeData(async (args) =>
    {
        // What the event listener is going to do when it triggers
        string content = $"{topic} topic: {args.ApplicationMessage.ConvertPayloadToString()}";
        await FileReadWriteHelper.WriteToReportAsync(content);
        Console.WriteLine(content);
    }, topic);
}

// Public to the topics
foreach (var tuple in contentToPublish)
{
    var topic = tuple.Item1;
    var content = tuple.Item2;
    await DataTransmissionHelper.PublishData(topic, content);
    Thread.Sleep(1000);
}

// Print the last messages
Console.WriteLine();
foreach (var topic in topics)
    Console.WriteLine($"{topic}-topic final message: {DataTransmissionHelper.GetLastMessageFromTopic(topic)}");

// End of the program
Console.WriteLine($"{Environment.NewLine}Press enter to close program");
Console.ReadLine();