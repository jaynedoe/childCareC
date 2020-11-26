using System;
using System.IO;
using System.Collections.Generic;

public class ChildCareCentre
{
    //fields

    //properties
    public string ServiceName { get; private set; }
    public string Suburb { get; private set; }
    public string Postcode { get; private set; }
    public string OverallRating { get; private set; }
    public string NumberOfApprovedPlaces { get; private set; }
    public int Code { get; private set; }
    public int CostPerDay { get; private set; }


    //constructors
    public ChildCareCentre(string serviceName, string suburb, string postcode, string overallRating, string numberOfApprovedPlaces, int code, int cost)
    {
        ServiceName = serviceName;
        Suburb = suburb;
        Postcode = postcode;
        OverallRating = overallRating;
        NumberOfApprovedPlaces = numberOfApprovedPlaces;
        Code = code;
        CostPerDay = cost;
    }

    //methods
    public void PrintSummary()
    {
        System.Console.WriteLine($"\nCentre Number {Code} - {ServiceName}");
        System.Console.WriteLine($"{Suburb}, {Postcode}");
        System.Console.WriteLine($"Cost Per Day ${CostPerDay}");
        System.Console.WriteLine($"{OverallRating}");
        System.Console.WriteLine($"Number of Approved Places: {NumberOfApprovedPlaces}\n");
    }
}