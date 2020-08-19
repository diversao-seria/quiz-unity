using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class AlternativeConverter : JsonConverter
{

    public override bool CanRead => true;
    public override bool CanWrite => false;

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        string json = (string) reader.Path;
        Debug.Log(" +++++++++++++ json: " + json + "\n" + "objectType:" + objectType.FullName) ;



        var result = JsonConvert.DeserializeObject(json, objectType);
        return result;
    }

    public override bool CanConvert(Type objectType)
    {
            return  true;
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }
}
