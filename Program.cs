using PubnubApi;
using System.Diagnostics;
using Microsoft.OpenApi.Models;

PNConfiguration pnConfiguration = new PNConfiguration(new UserId("myUniqueUserId"));
pnConfiguration.SubscribeKey = "sub-c-fd56b90e-dbc7-40b7-87c3-2922d55f5481";
pnConfiguration.PublishKey = "pub-c-4c0884fd-f78f-448d-a774-c84e4ab5c993";
Pubnub pubnub = new Pubnub(pnConfiguration);

Dictionary<string, string> message = new Dictionary<string, string>();
message.Add("msg", "Hello world");

pubnub.AddListener(new SubscribeCallbackExt(
    delegate (Pubnub pnObj, PNMessageResult<object> pubMsg)
    {
        if (pubMsg != null) {
            Console.WriteLine("In Example, SubscribeCallback received PNMessageResult");
            Console.WriteLine("In Example, SubscribeCallback messsage channel = " + pubMsg.Channel);
            Console.WriteLine("In Example, SubscribeCallback messsage channelGroup = " + pubMsg.Subscription);
            Console.WriteLine("In Example, SubscribeCallback messsage publishTimetoken = " + pubMsg.Timetoken);
            Console.WriteLine("In Example, SubscribeCallback messsage publisher = " + pubMsg.Publisher);
            Console.WriteLine("In Example, SubscribeCallback messsage message = " + pubMsg.Message);
            string jsonString = pubMsg.Message.ToString();
            Dictionary<string, string> msg = pubnub.JsonPluggableLibrary.DeserializeToObject<Dictionary<string,string>>(jsonString);
            Console.WriteLine("msg: " + msg["msg"]);
        }
    },
    delegate (Pubnub pnObj, PNPresenceEventResult presenceEvnt)
    {
        if (presenceEvnt != null) {
            Console.WriteLine("In Example, SubscribeCallback received PNPresenceEventResult");
            Console.WriteLine(presenceEvnt.Channel + " " + presenceEvnt.Occupancy + " " + presenceEvnt.Event);
        }
    },
    delegate (Pubnub pnObj, PNStatus pnStatus)
    {
        if (pnStatus.Category == PNStatusCategory.PNConnectedCategory) {
        pubnub.Publish()
        .Channel("my_channel")
        .Message(message)
        .Execute(new PNPublishResultExt((publishResult, publishStatus) => {
            if (!publishStatus.Error) {
                Console.WriteLine(string.Format("DateTime {0}, In Publish Example, Timetoken: {1}", DateTime.UtcNow, publishResult.Timetoken));
            } else {
                Console.WriteLine(publishStatus.Error);
                Console.WriteLine(publishStatus.ErrorData.Information);
            }
        }));
        }
    }
));

pubnub.Subscribe<string>().Channels(new string[]{"my_channel"}).Execute();

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
     c.SwaggerDoc("v1", new OpenApiInfo { Title = "PizzaStore API", Description = "Making the Pizzas you love", Version = "v1" });
});
    
var app = builder.Build();
    
if (app.Environment.IsDevelopment())
{
   app.UseSwagger();
   app.UseSwaggerUI(c =>
   {
      c.SwaggerEndpoint("/swagger/v1/swagger.json", "PizzaStore API V1");
   });
}

app.MapGet("/", () => "Hello World!");

app.MapPost("/publish", (string message) => {
    pubnub.Publish()
    .Channel("my_channel")
    .Message(message)
    .Execute(new PNPublishResultExt((publishResult, publishStatus) => {
            if (!publishStatus.Error) {
                Console.WriteLine(string.Format("DateTime {0}, In Publish Example, Timetoken: {1}", DateTime.UtcNow, publishResult.Timetoken));
            } else {
                Console.WriteLine(publishStatus.Error);
                Console.WriteLine(publishStatus.ErrorData.Information);
            }
        }));
});

app.Run();
