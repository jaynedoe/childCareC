using System;
using System.Collections.Generic;
using System.IO;

public class FamilyCentre
{
    
    //properties - the family centre 'has-a' family list and a base family
    
    private List<Family> familyList = new List<Family>();

    private Family baseFamily;


    //instantiate a new Family Centre object - create an object and read in family data at the same time

    public FamilyCentre()
    {
        ReadFamilyData();
    }

    //methods of the 'Family Centre' - it can read in family data from an external CSV file, write data back to the same file, display a menu and return the option selected as an integer, add, search, print all and remove families.

    private void ReadFamilyData()
    {
        string line, familyName, parent1Name, parent2Name, suburb, householdType;
        string[] splitLine;
        int noOfChildren, parent1Salary, parent2Salary;
    
        using (StreamReader reader = new StreamReader(new FileStream("newFamilyFile.csv", FileMode.Open)))
        {
            reader.ReadLine();

            line = reader.ReadLine();
            while (line != null)
            {
                try
                {
                    splitLine = line.Split(',');
                    familyName = splitLine[0];
                    parent1Name = splitLine[1];
                    noOfChildren = Convert.ToInt32(splitLine[2]);
                    suburb = splitLine[3];
                    parent1Salary = Convert.ToInt32(splitLine[4]);
                    householdType = splitLine[5];
                    parent2Name = splitLine[6];
                    parent2Salary = Convert.ToInt32(splitLine[7]);

                    if(householdType == "Single")
                    {
                        Family newFamily = new SingleFamily(familyName, parent1Name, noOfChildren, suburb, parent1Salary);
                        familyList.Add(newFamily);
                    } 
                    else
                    {
                        Family newFamily = new CoupleFamily(familyName, parent1Name, noOfChildren, suburb, parent1Salary, parent2Name, parent2Salary);
                        familyList.Add(newFamily);
                    }
                }
                catch (Exception e)
                {
                    System.Console.WriteLine(e.Message);;
                }
                line = reader.ReadLine();
            }
        }
    }

    private void WriteFamilyData()
    {
        System.Console.WriteLine("\nPlease wait while your changes upload...");
        
        using (StreamWriter writer = new StreamWriter("newFamilyFile.csv"))
        {
            writer.WriteLine();

            foreach (var item in familyList)
            {
                if(item.HouseholdType == "Couple")
                {
                    writer.WriteLine($"{item.FamilyName},{item.Parent1Name},{item.NumberOfChildren},{item.Suburb},{item.Parent1TaxableSalary},{item.HouseholdType},{(item as CoupleFamily).Parent2Name},{(item as CoupleFamily).Parent2TaxableSalary}");
                }
                else
                {
                    writer.WriteLine($"{item.FamilyName},{item.Parent1Name},{item.NumberOfChildren},{item.Suburb},{item.Parent1TaxableSalary},{item.HouseholdType},blank,{0}");
                }
             }
        }
        System.Console.WriteLine("Thanks, the database has been updated.  Please press enter to continue.");
        Console.ReadKey();
    }

    public int FamilyCentreMenu()
    {
        int option;
        
        do
        {
            System.Console.WriteLine($"\nPlease choose from the following options:");
            System.Console.WriteLine("\n1. Search for a family by name.");
            System.Console.WriteLine("2. Add in a new family.");
            System.Console.WriteLine("3. Remove an existing family.");
            System.Console.WriteLine("4. Show all families in the database.");
            System.Console.WriteLine("5. Return to the main menu.");
            try
            {
                option = Convert.ToInt32(Console.ReadLine());
            }
            catch (System.Exception)
            {
                System.Console.WriteLine("Invalid entry, please choose a number between 1 and 5.");
                option = 0;
            }
        } while (option < 1 || option > 5);

        return option;
    }

    public void AddNewProfile()
    {
        string familyName, parent1Name, parent2Name, suburb, household;
        int noOfChildren, parent1Salary, parent2Salary;

        //create a name for the family, check against existing
        System.Console.WriteLine($"\nYou have chosen to create a new family profile.  What is the name of your family?");

        do
        {
            familyName = Console.ReadLine();
            foreach (var item in familyList)
            {
                if(item.FamilyName == familyName)
                {
                    System.Console.WriteLine("\nThat family name is taken, please enter a different name:");
                    familyName = "wrong";
                }
            }
        } while (familyName == "wrong");    
  
        //single or couple?
        System.Console.WriteLine("\nAre you a single or couple household? Type 1 for single, 2 for couple:");
        
        do
        {
            household = Console.ReadLine();

            if(household != "1" && household != "2")
            {
                System.Console.WriteLine("Not a valid option, please enter 1 for a single household or 2 for a couple:");
                household = "3";
            }
            
        } while (household == "3");
  

        //obtain suburb details
        System.Console.WriteLine("\nWhat suburb do you live in?");
        suburb = Console.ReadLine();

        //obtain number of children
        do
        {
            System.Console.WriteLine("\nHow many children under 5 do you have?  Max 3:");
            try
            {
                noOfChildren = Convert.ToInt32(Console.ReadLine());
                if(noOfChildren < 1 || noOfChildren > 3)
                {
                    System.Console.WriteLine("Must be a number between 1 and 3:");
                    noOfChildren = 0;
                }
            }
            catch (System.Exception)
            {
                    System.Console.WriteLine("Not a valid response, please try again.");
                    noOfChildren = 0;
            }
            
        } while (noOfChildren == 0);

        //obtain parent details
        if(household == "1")
        {
            System.Console.WriteLine("\nWhat is the name of the parent in the family?");
            parent1Name = Console.ReadLine();

            System.Console.WriteLine($"\nWhat is {parent1Name}'s before tax annual salary, excluding super?");
            do
            {
                try
                {
                    parent1Salary = Convert.ToInt32(Console.ReadLine());                    
                }
                catch (System.Exception)
                {
                    System.Console.WriteLine("Not a valid response, please try again.");
                    parent1Salary = 0;
                }
            } while (parent1Salary == 0);

            Family newFamily = new SingleFamily(familyName, parent1Name, noOfChildren, suburb, parent1Salary);
            baseFamily = newFamily;
            familyList.Add(newFamily);
        } 
        else 
        {
            System.Console.WriteLine("\nWhat is the name of a parent in the family?");
            parent1Name = Console.ReadLine();

            System.Console.WriteLine($"\nHow much does {parent1Name} earn before tax per annum, excluding super?");
            do
            {
                try
                {
                    parent1Salary = Convert.ToInt32(Console.ReadLine());
                }
                catch (System.Exception)
                {
                    System.Console.WriteLine("Not a valid response, please try again.");
                    parent1Salary = 0;
                }
            } while (parent1Salary == 0);

            System.Console.WriteLine("\nWhat is the name the other parent in the family?");
            parent2Name = Console.ReadLine();

            System.Console.WriteLine($"\nHow much does {parent2Name} earn before tax per annum, excluding super?");
            do
            {
                try
                {
                    parent2Salary = Convert.ToInt32(Console.ReadLine());
                }
                catch (System.Exception)
                {
                    System.Console.WriteLine("Not a valid response, please try again.");
                    parent2Salary = 0;
                }
            } while (parent2Salary == 0);

            Family newFamily = new CoupleFamily(familyName, parent1Name, noOfChildren, suburb, parent1Salary, parent2Name, parent2Salary);
            baseFamily = newFamily;
            familyList.Add(newFamily);
        }
 
        //complete profile, show details of new family
        System.Console.WriteLine("\nThanks.  Here are the starting details of your new family.");
        baseFamily.PrintFamilyDetails();

        //save to database
        System.Console.WriteLine("\nDo you want to save this family to the database? Please enter Y or N:");
        string choice = Console.ReadLine();

        if(choice == "Y" || choice == "y") { WriteFamilyData(); } 
        else{ System.Console.WriteLine("Returning to the main menu..."); }
    }

    public Family SearchExistingProfile()
    {
        System.Console.WriteLine("\nPlease enter the name of the family:");
        string name = Console.ReadLine();

        foreach (var item in familyList)
        {
            if(item.FamilyName == name)
            {
                item.PrintFamilyDetails();
                return item;
            } 
        }
        System.Console.WriteLine("\nSorry, that family doesn't exist in the database.");
        return null;
    }

    public void PrintAllFamilies()
    {
        if(familyList.Count != 0)
        {
            foreach (var item in familyList)
            {
                item.PrintFamilyDetails();
            }
        }
        else
        {
            System.Console.WriteLine("\nThere are no families in the database.");
        }
    }

    public void RemoveProfile()
    {
        System.Console.WriteLine("\nYou've chosen to remove a family from the database.");
        string option;
        Family deleteFamily = SearchExistingProfile();
        
        if(deleteFamily != null)
        {            
            System.Console.WriteLine($"\nYou've chosen to remove {deleteFamily.FamilyName}.  Are you sure?  Type Y or N.");
            option = Console.ReadLine();
            
            if(option == "Y" || option == "y")
            {
                familyList.Remove(deleteFamily);
                WriteFamilyData();
            }
            else if(option == "N" || option == "n")
            {
                System.Console.WriteLine("\nReturning to the menu...");
            }
        } 
        else
        {
            System.Console.WriteLine("\n Deletion Failed.");
        } 
    }

}