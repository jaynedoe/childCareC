using System;
using System.IO;
using System.Collections.Generic;
//Child Care Search and Compare Program - Find and compare child care centres, select a preferred centre and estimate impact on finances of using that child care centre.
//where the program starts - control centre

public class Program
{
    //declare enumeration list of menu options
    
    public enum MenuOptions
    {
        ChildCareCentreManagement,
        FamilyProfiling,
        ScenarioBuilder,
        Exit
    }

    //display a menu, ask for a selection, return a 'MenuOptions' enumeration value based on the selection, includes validation

    private static MenuOptions ReadUserOption()
    {
        int result;

        do
        {
            System.Console.WriteLine("\nWelcome to the Program.  Please choose an option:\n");
            System.Console.WriteLine("1. Childcare Centre Management");
            System.Console.WriteLine("2. Family Profiling");
            System.Console.WriteLine("3. Scenario Comparator");
            System.Console.WriteLine("4. Exit the Program\n");

            try
            {
                result = Convert.ToInt32(Console.ReadLine());
            }
            catch (System.Exception)
            {
                
                System.Console.WriteLine("\nNot a valid option.");
                result = -1;
            }
            if (result < 1 || result > 4)
            {
                System.Console.WriteLine("\nNumber chosen must be between 1 and 4.  Please try again.");
            }

        } while (result < 1 || result > 4);

        return (MenuOptions)(result - 1);
    }

    //menu option - childcare centre management
    private static void ChildCareManagement(DataCentre newCentre)
    {
        int option;
        do
        {
            option = newCentre.DataCentreMenu();
            switch (option)
            {
                case 1:
                    newCentre.Search();
                    break;
                case 2:
                    newCentre.AddCentre();
                    break;
                case 3:
                    newCentre.RemoveCentre();
                    break;
                case 4:
                    System.Console.WriteLine("\nReturning to Main Menu...");
                    break;
                default:
                    System.Console.WriteLine("Default");
                    break;
            }

        } while (option != 4);

    }


    //menu option - family profiling
    private static void FamilyProfileManagement(FamilyCentre familyList)
    {
        int option;

        do
        {
            option = familyList.FamilyCentreMenu();        
            switch (option)
            {
                case 1:
                    familyList.SearchExistingProfile();
                    break;
                case 2:
                    familyList.AddNewProfile();
                    break;
                case 3:
                    familyList.RemoveProfile();
                    break;
                case 4:
                    familyList.PrintAllFamilies();
                    break;
                case 5:
                    System.Console.WriteLine("\nReturning to the main menu....");
                    break;
                default:
                    System.Console.WriteLine("Default");
                    break;
            }
        } while (option != 5);
    }
     
    //menu option - care scenarios
    private static void ScenarioManagement(FamilyCentre newFamilyCentre, DataCentre newDataCentre)
    {
        //select base family to compare from family centre
        System.Console.WriteLine("\nTo make a comparison, you need to select an existing family profile.");
        Family baseFamily = FindFamily(newFamilyCentre);
        
        if(baseFamily == null)
        {
            System.Console.WriteLine("\nReturning to the main menu....");
        }
        else 
        {
            //create base scenario for comparison
            CareScenario baseScenario = new CareScenario(baseFamily);
            baseScenario.RegisterCareDetails(newDataCentre);
            baseScenario.RegisterIncomeDetails();
            
            System.Console.WriteLine("\nThanks.  Please now enter in your alternative child care scenario.");

            //create alternate scenario for comparison, and print results

            //copy existing family into new object
            Family altest = baseFamily.DeepCopy();

            CareScenario alternativeScenario = new CareScenario(altest);
                
            alternativeScenario.RegisterCareDetails(newDataCentre);
            alternativeScenario.RegisterIncomeDetails();

            CalculationCentre newCalcCentre = new CalculationCentre(baseScenario, alternativeScenario);

            if(baseFamily.HouseholdType == "Couple")
            {
                newCalcCentre.PrintComparisonCouple(baseFamily, altest);
            } 
            else if(baseFamily.HouseholdType == "Single")
            {
                newCalcCentre.PrintComparisonSingle(baseFamily, altest);
            }
        }    
    }

    //private method to search the current family list and return a family object if found in the database

    private static Family FindFamily(FamilyCentre newCentre)
    {
        return newCentre.SearchExistingProfile();
    }

    public static void Main()
    {
        DataCentre newDataCentre = new DataCentre();
        FamilyCentre newFamilyCentre = new FamilyCentre();

        MenuOptions option;

        do
        {
            option = ReadUserOption();
            
            switch (option)
            {
                case MenuOptions.ChildCareCentreManagement:
                    ChildCareManagement(newDataCentre);
                    break;
                case MenuOptions.FamilyProfiling:
                    FamilyProfileManagement(newFamilyCentre);
                    break;
                case MenuOptions.ScenarioBuilder:
                    ScenarioManagement(newFamilyCentre, newDataCentre);
                    break;
                case MenuOptions.Exit:
                    System.Console.WriteLine("\nGoodbye!");
                    break;
                default:
                    System.Console.WriteLine("Default");
                break;
            }
        } while (option != MenuOptions.Exit);
 
    } 
}
