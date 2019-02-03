using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using System.Net;
using System;
using System.IO;
using Newtonsoft.Json;

public class RatingSummary
{
    public string name { get; set; }
    public int rating { get; set; }
    public string colorStyle { get; set; }
}

public class Achievement
{
    public DateTime date { get; set; }
    public string description { get; set; }
}

public class ServerInformation
{
    public string serverName { get; set; }
    public string apiVersion { get; set; }
    public int requestDuration { get; set; }
    public long currentTime { get; set; }
}

public class ReceivedParams
{
    public string apiVersion { get; set; }
    public string handle { get; set; }
    public string action { get; set; }
}

public class RequesterInformation
{
    public string id { get; set; }
    public string remoteIP { get; set; }
    public ReceivedParams receivedParams { get; set; }
}

public class RootObject
{
    public string handle { get; set; }
    public string country { get; set; }
    public DateTime memberSince { get; set; }
    public string quote { get; set; }
    public string photoLink { get; set; }
    public bool copilot { get; set; }
    public List<RatingSummary> ratingSummary { get; set; }
    public List<Achievement> Achievements { get; set; }
    public ServerInformation serverInformation { get; set; }
    public RequesterInformation requesterInformation { get; set; }
}

public class sphereColllision : MonoBehaviour
{
    //public string topcoderHandle = "tourist";
    //public string url = String.Format("https://api.topcoder.com/v2/users/{0}", topcoderHandle);
    public string url = "https://api.topcoder.com/v2/users/tourist";
    public int x;
    public float y, z;
    public int scalingFactorX = 3718;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision c)
    {
        if (c.gameObject.name == "player")
        {
            Destroy(this.gameObject);
            RootObject json = getJson(url);
            x = json.ratingSummary[0].rating;

            foreach (var element in json.Achievements)
            {
                //print(element.description);
                foreach ( string word in element.description.Split())
                {
                    if (word.Length > 3)
                    {
                        if (word.ToLower().Substring(0, 3) == "win") y++;
                    }
                    if (word.ToLower() == "marathon") z++;                   
                    //print(word);
                }
            }
            int scaled_X = (int)Math.Floor((float)(x / scalingFactorX))  * 5 ;
            Metrics metric = new Metrics(scaled_X, y/2, z/2, 
                String.Format("user has a score of {0} for problemSolving",x), 
                String.Format("user has a score of {0} for competetive", y), 
                String.Format("user has a score of {0} for competetive", z));
            print(metric.problemSolving);
            print(metric.competetive);
            print(metric.persistant);
            print(metric.reasons[0]);
            print(metric.reasons[1]);
            print(metric.reasons[2]);
        }
    }

    RootObject getJson(string url2)
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(String.Format(url2));
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string jsonResponse = reader.ReadToEnd();
        RootObject obj = JsonConvert.DeserializeObject<RootObject>(jsonResponse);
        return obj;
    }

}

public struct Metrics
{
    public int problemSolving;
    public float competetive;
    public float persistant;
    public List<string> reasons;

    public Metrics(int x, float y, float z, string r1, string r2, string r3)
    {
        problemSolving = x;
        competetive = y;
        persistant = z;
        reasons = new List<string>();
        reasons.Add(r1);
        reasons.Add(r2);
        reasons.Add(r3);

    }
}
