using System;
using System.IO;
using System.Collections.Generic;

public abstract class Family
{
    //fields

    //properties

    public string FamilyName { get; private set; }
    public string Parent1Name { get; private set; }
    public int Parent1TaxableSalary { get; private set; }
    public int NumberOfChildren { get; private set; }
    public string Suburb { get; private set; }
    public int FamilyAdjustedTaxableIncome { get; protected set; }
    public string HouseholdType { get; protected set; }
    
    public Family(string familyName, string parentName, int noOfChildren, string suburb, int taxableIncome)
    {
        NumberOfChildren = noOfChildren;
        Suburb = suburb;
        FamilyName = familyName;
        Parent1TaxableSalary = taxableIncome;
        Parent1Name = parentName;
    }
    
    //methods
    public abstract void PrintFamilyDetails();

    public abstract void PrintCareSummary();

    public virtual void ConfirmIncome()
    {
        System.Console.WriteLine($"\n{Parent1Name} currently earns {Parent1TaxableSalary} per annum.  Would you like to change this? Please enter Y or N.");
        string choice = Console.ReadLine();
        if(choice == "Y" || choice == "y")
        {
            do
            {
                System.Console.WriteLine($"\nPlease enter a new gross annual salary for {Parent1Name}");
                try
                {
                    Parent1TaxableSalary = Convert.ToInt32(Console.ReadLine());
                }
                catch (System.Exception)
                {
                    System.Console.WriteLine("Invalid entry, please try again.");    
                    Parent1TaxableSalary = -1;
                }
            } while (Parent1TaxableSalary < 0);
        } 
        else
        {
            System.Console.WriteLine($"\n{Parent1Name}'s salary left unchanged.");
        }
    }

    public Family DeepCopy()
    {
        Family other = (Family)this.MemberwiseClone();
        return other;
    }

}

public class SingleFamily : Family
{
    public SingleFamily(string familyName, string parentName, int noOfChildren, string suburb, int taxableIncome) : base (familyName, parentName, noOfChildren, suburb, taxableIncome)
    {
        HouseholdType = "Single";
        FamilyAdjustedTaxableIncome = taxableIncome;
    }

    public override void PrintFamilyDetails()
    {
        System.Console.WriteLine($"\nFamily Name: {FamilyName}");
        System.Console.WriteLine($"Parent: {Parent1Name}");
        System.Console.WriteLine($"Living in: {Suburb}");
        System.Console.WriteLine($"Children Under 5: {NumberOfChildren}");
        System.Console.WriteLine($"Total Annual Taxable Income: {Parent1TaxableSalary}.");
    }

    public override void PrintCareSummary()
    {
        System.Console.WriteLine($"\n{Parent1Name} is earning {Parent1TaxableSalary} per annum.");
    }

    public override void ConfirmIncome()
    {
        base.ConfirmIncome();
    }
}

public class CoupleFamily : Family
{
    public string Parent2Name { get; set; }
    public int Parent2TaxableSalary { get; set; }

    public CoupleFamily(string familyName, string parent1Name, int noOfChildren, string suburb, int taxableIncome, string parent2Name, int parent2TaxableSalary) : base (familyName, parent1Name, noOfChildren, suburb, taxableIncome)
    {
        Parent2Name = parent2Name;
        Parent2TaxableSalary = parent2TaxableSalary;
        HouseholdType = "Couple";
        FamilyAdjustedTaxableIncome = taxableIncome + parent2TaxableSalary;
    }

    public override void PrintFamilyDetails()
    {
        System.Console.WriteLine($"\nFamily Name: {FamilyName}");
        System.Console.WriteLine($"Parents: {Parent1Name} and {Parent2Name}");
        System.Console.WriteLine($"Lives in: {Suburb}");
        System.Console.WriteLine($"Children Under 5: {NumberOfChildren}");
        System.Console.WriteLine($"Combined Annual Taxable Income: {Parent1TaxableSalary + Parent2TaxableSalary}");
    }

    public override void PrintCareSummary()
    {
        System.Console.WriteLine($"{Parent1Name} is earning {Parent1TaxableSalary} per annum, and {Parent2Name} is earning {Parent2TaxableSalary} per annum.");
    }

    public override void ConfirmIncome()
    {
        base.ConfirmIncome();

        System.Console.WriteLine($"\n{Parent2Name} currently earns {Parent2TaxableSalary} per annum.  Would you like to change this? Please enter Y or N.");
        string choice2 = Console.ReadLine();
        if(choice2 == "Y" || choice2 == "y")
        {
           do
            {
                System.Console.WriteLine($"\nPlease enter a new gross annual salary for {Parent2Name}");
                try
                {
                    Parent2TaxableSalary = Convert.ToInt32(Console.ReadLine());
                }
                catch (System.Exception)
                {
                    System.Console.WriteLine("Invalid entry, please try again.");    
                    Parent2TaxableSalary = -1;
                }
            } while (Parent2TaxableSalary < 0);
        } 
        else
        {
            System.Console.WriteLine($"\n{Parent2Name}'s salary left unchanged.");
        }
    }
}