using System;
using System.IO;
using System.Collections.Generic;

public class DataCentre
{
    //fields
    private List<ChildCareCentre> centres = new List<ChildCareCentre>();

    //properties

    public ChildCareCentre PreferredCentre { get; private set; }

    //constructors
    public DataCentre()
    {
        ReadData();
    }

    //methods
    private void ReadData()
    {
        string line, serviceName, suburb, postcode, numberOfApprovedPlaces, overallRating;
        string[] splitLine;
        int code, cost;
    
        using (StreamReader reader = new StreamReader(new FileStream("newFile.csv", FileMode.Open)))
        {
            reader.ReadLine();

            line = reader.ReadLine();
            while (line != null)
            {
                try
                {
                    splitLine = line.Split(',');
                    serviceName = splitLine[0];
                    suburb = splitLine[1];
                    postcode = splitLine[2];
                    numberOfApprovedPlaces = splitLine[3];
                    overallRating = splitLine[4];
                    code = Convert.ToInt32(splitLine[5]);
                    cost = Convert.ToInt32(splitLine[6]);
                    
                    ChildCareCentre newCentre = new ChildCareCentre(serviceName, suburb, postcode, overallRating, numberOfApprovedPlaces, code, cost);

                    centres.Add(newCentre);
                }
                catch (Exception e)
                {
                    System.Console.WriteLine(e.Message);;
                }
                line = reader.ReadLine();
            }
        }
    }

    //write data

    private void WriteData()
    {
        System.Console.WriteLine("\nPlease wait while your changes upload...");
        
        using (StreamWriter writer = new StreamWriter("newFile.csv"))
        {
            writer.WriteLine();

            foreach (var item in centres)
            {
                writer.WriteLine($"{item.ServiceName},{item.Suburb},{item.Postcode},{item.NumberOfApprovedPlaces},{item.OverallRating},{item.Code},{item.CostPerDay}");
            }
        }
        System.Console.WriteLine("Thanks, the database has been updated.  Please press enter to continue.");
        Console.ReadKey();
    }

    //sub menu for data centre
    public int DataCentreMenu()
    {
        int option;
        System.Console.WriteLine("\nPlease choose from the following options:");        

        do
        {
            System.Console.WriteLine("\n1. Search for centres based on selected criteria");
            System.Console.WriteLine("2. Add in a new custom centre");
            System.Console.WriteLine("3. Remove an existing centre");
            System.Console.WriteLine("4. Return to the main menu");
            try
            {
                option = Convert.ToInt32(Console.ReadLine());
            }
            catch (System.Exception)
            {
                System.Console.WriteLine("\nInvalid entry.");
                option = 0;
            }
            if(option < 1 || option > 4)
            {
                System.Console.WriteLine("Please try again, number chosen must be between 1 and 4:");
            }
        } while (option < 1 || option > 4);

        return option;
    }

    //add in centre

    public void AddCentre()
    {
        int newCode = 0;
        string overallRating;
        int costPerDay;
        bool check;
        
        System.Console.WriteLine("\nPlease enter the name of your new centre:");
        string name = Console.ReadLine();
        System.Console.WriteLine("\nPlease enter the suburb of your new centre:");
        string suburb = Console.ReadLine();
        System.Console.WriteLine("\nPlease enter the postcode of your new centre:");
        string postcode = Console.ReadLine();
        
        System.Console.WriteLine("\nPlease enter the overall rating of your new centre:");
        System.Console.WriteLine("\nFor 'Exceeding NQS, type in 1.");
        System.Console.WriteLine("For 'Meeting NQS', type in 2.");
        System.Console.WriteLine("For 'Working Towards NQS', type in 3.");
        
        string choice = Console.ReadLine();
        if(choice == "1"){ overallRating = "Exceeding NQS";} 
        else if(choice == "2"){overallRating = "Meeting NQS";}
        else{overallRating = "Working Towards NQS";}

        System.Console.WriteLine("\nPlease enter the number of approved places of your new centre:");
        string numberOfApprovedPlaces = Console.ReadLine();
        
        do
        {
            System.Console.WriteLine("\nPlease enter the cost per day of your new centre:");
            check = false;

            try
            {
                costPerDay = Convert.ToInt32(Console.ReadLine());
            }
            catch (System.Exception)
            {
                
                System.Console.WriteLine("Invalid response, please enter a valid number.");
                check = true;
                costPerDay = 0;
            }
            
        } while (check);
        
        if(centres.Count != 0)
        {
            int item = (centres.Count - 1);
            newCode = centres[item].Code+1;         
        }
        else
        {
            newCode++;
        } 
  
        ChildCareCentre addCentre = new ChildCareCentre(name, suburb, postcode, overallRating, numberOfApprovedPlaces, newCode, costPerDay);
        centres.Add(addCentre);

        System.Console.WriteLine("\nThanks. Here are the details of the centre you just added.");
        addCentre.PrintSummary();

        System.Console.WriteLine("\nDo you want to save this centre to the database?  Please type Y or N.");
        choice = Console.ReadLine();

        if(choice == "Y" || choice == "y")
        {
            WriteData();
        }
        else
        {
            System.Console.WriteLine("Returning to the main menu...");
        }
    }


    //remove centres

    public void RemoveCentre()
    {
        bool check;
        int deleteCode;
        
        System.Console.WriteLine("\nYou've chosen to remove a child care centre from the database.  Please enter the code of the centre you'd like to delete.");

        do
        {
            check = false;
            try
            {
                deleteCode = Convert.ToInt32(Console.ReadLine());
            }
            catch (System.Exception)
            {
                System.Console.WriteLine($"\nInvalid code entry.  Please enter a number between 1 and {centres.Count}:");
                check = true;
                deleteCode = 0;                
            }
        } while (check);

        ChildCareCentre deleteCentre;
        string option;

        deleteCentre = Search(deleteCode);

        if(deleteCentre != null)
        {            
            System.Console.WriteLine($"\nYou've chosen to remove {deleteCentre.ServiceName}.  Are you sure?  Type Y or N.");
            option = Console.ReadLine();
            if(option == "Y" || option == "y")
            {
                centres.Remove(deleteCentre);
                WriteData();
            }
            else if(option == "N" || option == "n")
            {
                System.Console.WriteLine("\nReturning to the menu.");
            }
        } 
        else
        {
            System.Console.WriteLine("\nCannot find centre to delete.");
        } 
    }


    //search for centres
    public void Search()
    {
        List<ChildCareCentre> searchResult = new List<ChildCareCentre>();

        System.Console.WriteLine("\nWould you like to search by code or by criteria?");
        System.Console.WriteLine("\n1 - Code");
        System.Console.WriteLine("2 - Criteria");
        string option;
        int code, maxCostPerDay; 
        bool check;
 
        option = Console.ReadLine();

        if(option == "1")
        {
            System.Console.WriteLine("\nPlease enter a code to search for:"); 
            do
            {
                check = false;
                try
                {
                    code = Convert.ToInt32(Console.ReadLine());
                }
                catch (System.Exception)
                {
                    System.Console.WriteLine($"Not a valid entry.  Please enter a number between 1 and {centres.Count}:");
                    check = true;
                    code = 0;
                }
            } while (check);

            ChildCareCentre selectedCenter = Search(code);

            if(selectedCenter != null)
            {
                selectedCenter.PrintSummary();                
            }
            else
            {
                System.Console.WriteLine("\nThat code is not in the database.");
            }    
        } 
        else if(option == "2")
        {
            System.Console.WriteLine("\nPlease enter a postcode to search for:");
            string postcode = Console.ReadLine();

            //select rating
            System.Console.WriteLine("\nPlease enter an overall rating:");
            System.Console.WriteLine("For Exceeding NQS, select 1");
            System.Console.WriteLine("For Meeting NQS, select 2");
            System.Console.WriteLine("For Working Towards NQS, select 3");
            System.Console.WriteLine("For any rating, select 4");
            
            string overallRating;
            string rating = Console.ReadLine();
            switch (rating)
            {
                case "1":
                overallRating = "Exceeding NQS";
                break;
                case "2":
                overallRating = "Meeting NQS";
                break;
                case "3":
                overallRating = "Working Towards NQS";
                break;
                case "4":
                overallRating = null;
                break;   
                default:
                overallRating = "Meeting NQS";
                break;         
            }

            //find costs

            do
            {
                System.Console.WriteLine("\nPlease enter the maximum cost of childcare per day you can afford.");
                try
                {
                    maxCostPerDay = Convert.ToInt32(Console.ReadLine());
                }
                catch (System.Exception)
                {
                    maxCostPerDay = 0;
                    System.Console.WriteLine("Invalid entry.");
                }
            } while (maxCostPerDay == 0);

            //run search based on criteria
            searchResult = Search(postcode, overallRating, maxCostPerDay);

            if(searchResult.Count == 0)
            {
                System.Console.WriteLine("\nSorry, no centres match your criteria.");
            } 
            else 
            {
                System.Console.WriteLine("\nThe following centres match your criteria:");
            }

            //list matching centres
            foreach (var item in searchResult)
            {
                item.PrintSummary();
            }
        } 
        else 
        {
            System.Console.WriteLine("\nNot a valid option.");
        }        
    }

    private ChildCareCentre Search(int code)
    {
        foreach (var item in centres)
        {
            if(item.Code == code) { return item; }
        }

        return null;

    }

    private List<ChildCareCentre> Search(string postcode, string overallRating, int maxCostPerDay)
    {
        List<ChildCareCentre> searchResults = new List<ChildCareCentre>();

        if(overallRating == null)
        {
            foreach (var item in centres)
            {
                if(item.Postcode == postcode && item.CostPerDay <= maxCostPerDay)
                {
                    searchResults.Add(item);
                }
            }
        }
        else
        {
            foreach (var item in centres)
            {
                if(item.Postcode == postcode && item.OverallRating == overallRating && item.CostPerDay <= maxCostPerDay)
                {
                    searchResults.Add(item);
                }
            }
        }
        return searchResults;
    }

    //set preferred centre

    public ChildCareCentre SelectPreferredCentre()
    {
        int preferred;
        bool centreFound = false;

        do
        {
            System.Console.WriteLine("\nPlease enter the centre number of your preferred provider.");
            try
            {
                preferred = Convert.ToInt32(Console.ReadLine());
            }
            catch (System.Exception)
            {             
                System.Console.WriteLine("Invalid response, please enter a valid number.");
                preferred = 0;
            }
        } 
        while (preferred == 0);
       
        foreach (var item in centres)
        {
            if(preferred == item.Code)
            {
               PreferredCentre = item;
               centreFound = true;
            } 
        }
        
        if(centreFound)
        {
            System.Console.WriteLine("\nThanks.  The details of your preferred provider are:");
            PreferredCentre.PrintSummary();
            return PreferredCentre;
        } 
        else
        {
            System.Console.WriteLine("\nSorry, that centre number could not be found.");
        }
        return null;
    }
}