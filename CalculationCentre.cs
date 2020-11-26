using System;
using System.Collections.Generic;
using System.IO;

public delegate double PerformCalculation(int calc11);
public delegate double Perform2Calculation(int calc21, int calc22); 
public delegate double Perform3Calculation(int calc31, int calc32, int calc33);
public delegate double Perform4Calculation(int calc41, int calc42, int calc43, CareScenario scenario2);
public delegate double Perform5Calculation(int calc51, int calc52, CareScenario scenario3);
public delegate double Perform6Calculation(CareScenario scenario);
public class CalculationCentre
{
    public CareScenario BaseScenario { get; private set; }

    public CareScenario AlternativeScenario { get; private set; }

    public CalculationCentre(CareScenario baseScenario, CareScenario altScenario)
    {
        BaseScenario = baseScenario;
        AlternativeScenario = altScenario;
    }

    //methods

    public void PrintComparisonSingle(Family baseFamily, Family alternate)
    {
        PerformCalculation LITO = CalculateLITO;
        PerformCalculation MITO = CalculateMITO;
        PerformCalculation Medicare = CalculateMedicare;
        PerformCalculation GTP = GrossTaxPayable;
        PerformCalculation FTBB = CalculateFTBB;
        PerformCalculation nTax = (salary => {
            double result = GTP(salary) + Medicare(salary) - LITO(salary) - MITO(salary);
            if(result < 0){return 0;}else return result;});

        Perform2Calculation FTBA = CalculateFTBA;
        Perform2Calculation calcNI = (salary, children) => salary - nTax(salary) + FTBA(salary, children) + FTBB(salary);

        Perform6Calculation childCare = scenario => (scenario.DailyCostOfChildcare * scenario.DaysInCare) * 52;
        Perform6Calculation subsidy = scenario => 0.5 * childCare(scenario);
        Perform6Calculation totalCC = scenario => childCare(scenario) - subsidy(scenario);

        Perform5Calculation netSD = (salary, children, scenario) => calcNI(salary, children) - totalCC(scenario);
        
        System.Console.WriteLine($"\n|--------------------------------------------------------------------------------------------------------");
        System.Console.WriteLine($"| BASE SCENARIO SUMMARY");
        System.Console.WriteLine($"|");
        System.Console.WriteLine($"| {baseFamily.Parent1Name}");
        System.Console.WriteLine($"|   Taxable Income               {BaseScenario.MyFamily.Parent1TaxableSalary}");
        System.Console.WriteLine($"|   Gross Tax Payable            {GTP(BaseScenario.MyFamily.Parent1TaxableSalary)}");
        System.Console.WriteLine($"|   Medicare Levy                {Medicare(BaseScenario.MyFamily.Parent1TaxableSalary)}");
        System.Console.WriteLine($"|   Low Income Offset            {LITO(BaseScenario.MyFamily.Parent1TaxableSalary)}");  
        System.Console.WriteLine($"|   Mid Income Offset            {MITO(BaseScenario.MyFamily.Parent1TaxableSalary)}");  
        System.Console.WriteLine($"| Net Tax Payable                {nTax(BaseScenario.MyFamily.Parent1TaxableSalary)}");  
        System.Console.WriteLine($"|");  
        System.Console.WriteLine($"|   Family Tax A                 {Convert.ToInt32(FTBA(BaseScenario.MyFamily.Parent1TaxableSalary, BaseScenario.MyFamily.NumberOfChildren))}");  
        System.Console.WriteLine($"|   Family Tax B                 {Convert.ToInt32(FTBB(BaseScenario.MyFamily.Parent1TaxableSalary))}");  
        System.Console.WriteLine($"| Total Centrelink               {Convert.ToInt32(FTBA(BaseScenario.MyFamily.Parent1TaxableSalary, BaseScenario.MyFamily.NumberOfChildren) + FTBB(BaseScenario.MyFamily.Parent1TaxableSalary))}");  
        System.Console.WriteLine($"|");  
        System.Console.WriteLine($"| NET INCOME                     {Convert.ToInt32(calcNI(BaseScenario.MyFamily.Parent1TaxableSalary, BaseScenario.MyFamily.NumberOfChildren))}");  
        System.Console.WriteLine($"|");  
        System.Console.WriteLine($"|   Childcare Centre Expenses    {childCare(BaseScenario)}");  
        System.Console.WriteLine($"|   Less Childcare Subsidy       {subsidy(BaseScenario)}");  
        System.Console.WriteLine($"| Total Childcare Expenses       {totalCC(BaseScenario)}");  
        System.Console.WriteLine($"|");  
        System.Console.WriteLine($"| NET SURPLUS/DEFICIT            {Convert.ToInt32(netSD(BaseScenario.MyFamily.Parent1TaxableSalary, BaseScenario.MyFamily.NumberOfChildren, BaseScenario))}");  
        System.Console.WriteLine($"| Net Income per Fortnight       {Convert.ToInt32(netSD(BaseScenario.MyFamily.Parent1TaxableSalary, BaseScenario.MyFamily.NumberOfChildren, BaseScenario)) / 26}");  
        System.Console.WriteLine($"|---------------------------------------------------------------------------------------------------------");    

        System.Console.WriteLine($"|--------------------------------------------------------------------------------------------------------");
        System.Console.WriteLine($"| ALTERNATIVE SCENARIO SUMMARY");
        System.Console.WriteLine($"|");
        System.Console.WriteLine($"| {baseFamily.Parent1Name}");
        System.Console.WriteLine($"|   Taxable Income               {AlternativeScenario.MyFamily.Parent1TaxableSalary}");
        System.Console.WriteLine($"|   Gross Tax Payable            {GTP(AlternativeScenario.MyFamily.Parent1TaxableSalary)}");
        System.Console.WriteLine($"|   Medicare Levy                {Medicare(AlternativeScenario.MyFamily.Parent1TaxableSalary)}");
        System.Console.WriteLine($"|   Low Income Offset            {LITO(AlternativeScenario.MyFamily.Parent1TaxableSalary)}");  
        System.Console.WriteLine($"|   Mid Income Offset            {MITO(AlternativeScenario.MyFamily.Parent1TaxableSalary)}");  
        System.Console.WriteLine($"| Net Tax Payable                {nTax(AlternativeScenario.MyFamily.Parent1TaxableSalary)}");  
        System.Console.WriteLine($"|");  
        System.Console.WriteLine($"|   Family Tax A                 {Convert.ToInt32(FTBA(AlternativeScenario.MyFamily.Parent1TaxableSalary, AlternativeScenario.MyFamily.NumberOfChildren))}");  
        System.Console.WriteLine($"|   Family Tax B                 {Convert.ToInt32(FTBB(AlternativeScenario.MyFamily.Parent1TaxableSalary))}");  
        System.Console.WriteLine($"| Total Centrelink               {Convert.ToInt32(FTBA(AlternativeScenario.MyFamily.Parent1TaxableSalary, AlternativeScenario.MyFamily.NumberOfChildren) + FTBB(AlternativeScenario.MyFamily.Parent1TaxableSalary))}");  
        System.Console.WriteLine($"|");    
        System.Console.WriteLine($"| NET INCOME                     {Convert.ToInt32(calcNI(AlternativeScenario.MyFamily.Parent1TaxableSalary, AlternativeScenario.MyFamily.NumberOfChildren))}");  
        System.Console.WriteLine($"|");  
        System.Console.WriteLine($"|   Childcare Centre Expenses    {childCare(AlternativeScenario)}");  
        System.Console.WriteLine($"|   Less Childcare Subsidy       {subsidy(AlternativeScenario)}");  
        System.Console.WriteLine($"| Total Childcare Expenses       {totalCC(AlternativeScenario)}");  
        System.Console.WriteLine($"|");  
        System.Console.WriteLine($"| NET SURPLUS/DEFICIT            {Convert.ToInt32(netSD(AlternativeScenario.MyFamily.Parent1TaxableSalary, AlternativeScenario.MyFamily.NumberOfChildren, AlternativeScenario))}");  
        System.Console.WriteLine($"| Net Income per Fortnight       {Convert.ToInt32(netSD(AlternativeScenario.MyFamily.Parent1TaxableSalary, AlternativeScenario.MyFamily.NumberOfChildren, AlternativeScenario)) / 26}");  
        System.Console.WriteLine($"|---------------------------------------------------------------------------------------------------------");     

        System.Console.WriteLine("\nBASE SCENARIO");
        BaseScenario.PrintCareDetails();
        System.Console.WriteLine($"Net income per fortnight in this scenario is {Convert.ToInt32(netSD(BaseScenario.MyFamily.Parent1TaxableSalary, BaseScenario.MyFamily.NumberOfChildren, BaseScenario)) / 26}");
        System.Console.WriteLine("\nALTERNATE SCENARIO");
        AlternativeScenario.PrintCareDetails();
        System.Console.WriteLine($"Net income per fortnight in this scenario is {Convert.ToInt32(netSD(AlternativeScenario.MyFamily.Parent1TaxableSalary, AlternativeScenario.MyFamily.NumberOfChildren, AlternativeScenario)) / 26}");
        System.Console.WriteLine($"|---------------------------------------------------------------------------------------------------------");    
        System.Console.WriteLine($"|---------------------------------------------------------------------------------------------------------");   
    }

    public void PrintComparisonCouple(Family baseFamily, Family alternate)
    {
        int parent2TS1 = (baseFamily as CoupleFamily).Parent2TaxableSalary;
        int parent2TS2 = (alternate as CoupleFamily).Parent2TaxableSalary;

        PerformCalculation LITO = CalculateLITO;
        PerformCalculation MITO = CalculateMITO;
        PerformCalculation Medicare = CalculateMedicare;
        PerformCalculation GTP = GrossTaxPayable;
        PerformCalculation nTax = (salary => {
            double result = GTP(salary) + Medicare(salary) - LITO(salary) - MITO(salary);
            if(result < 0){return 0;}else return result;});

        Perform2Calculation FTBA = CalculateFTBA;
        Perform2Calculation FTBB = CalculateFTBB;
        
        Perform3Calculation calcNI = (salary, salary2, children) => salary + salary2 - nTax(salary) - nTax(salary2) + FTBA(salary+salary2, children) + FTBB(salary, salary2);
        
        Perform6Calculation childCare = scenario => (scenario.DailyCostOfChildcare * scenario.DaysInCare) * 52;
        Perform6Calculation subsidy = scenario => 0.5 * childCare(scenario);
        Perform6Calculation totalCC = scenario => childCare(scenario) - subsidy(scenario);

        Perform4Calculation netSD = (test1, test2, test3, scenario) => calcNI(test1, test2, test3) - totalCC(scenario);
       
        System.Console.WriteLine($"\n|--------------------------------------------------------------------------------------------------------");
        System.Console.WriteLine($"| BASE SCENARIO SUMMARY");
        System.Console.WriteLine($"|");
        System.Console.WriteLine($"| {baseFamily.Parent1Name}");
        System.Console.WriteLine($"|   Taxable Income               {BaseScenario.MyFamily.Parent1TaxableSalary}");
        System.Console.WriteLine($"|   Gross Tax Payable            {GTP(BaseScenario.MyFamily.Parent1TaxableSalary)}");
        System.Console.WriteLine($"|   Medicare Levy                {Medicare(BaseScenario.MyFamily.Parent1TaxableSalary)}");
        System.Console.WriteLine($"|   Low Income Offset            {LITO(BaseScenario.MyFamily.Parent1TaxableSalary)}");  
        System.Console.WriteLine($"|   Mid Income Offset            {MITO(BaseScenario.MyFamily.Parent1TaxableSalary)}");  
        System.Console.WriteLine($"| Net Tax Payable                {nTax(BaseScenario.MyFamily.Parent1TaxableSalary)}");  
        System.Console.WriteLine($"|");
        System.Console.WriteLine($"| {(baseFamily as CoupleFamily).Parent2Name}");
        System.Console.WriteLine($"|   Taxable Income               {parent2TS1}");
        System.Console.WriteLine($"|   Gross Tax Payable            {GTP(parent2TS1)}");
        System.Console.WriteLine($"|   Medicare Levy                {Medicare(parent2TS1)}");     
        System.Console.WriteLine($"|   Low Income Offset            {LITO(parent2TS1)}");  
        System.Console.WriteLine($"|   Mid Income Offset            {MITO(parent2TS1)}");  
        System.Console.WriteLine($"| Net Tax Payable                {nTax(parent2TS1)}");
        System.Console.WriteLine($"|");  
        System.Console.WriteLine($"|   Family Tax A                 {Convert.ToInt32(FTBA(BaseScenario.MyFamily.Parent1TaxableSalary + parent2TS1, BaseScenario.MyFamily.NumberOfChildren))}");  
        System.Console.WriteLine($"|   Family Tax B                 {Convert.ToInt32(FTBB(BaseScenario.MyFamily.Parent1TaxableSalary, parent2TS1))}");  
        System.Console.WriteLine($"| Total Centrelink               {Convert.ToInt32(FTBA(BaseScenario.MyFamily.Parent1TaxableSalary + parent2TS1, BaseScenario.MyFamily.NumberOfChildren) + FTBB(BaseScenario.MyFamily.Parent1TaxableSalary, parent2TS1))}");  
        System.Console.WriteLine($"|");  
        System.Console.WriteLine($"| NET INCOME                     {Convert.ToInt32(calcNI(BaseScenario.MyFamily.Parent1TaxableSalary, parent2TS1, BaseScenario.MyFamily.NumberOfChildren))}");  
        System.Console.WriteLine($"|");  
        System.Console.WriteLine($"|   Childcare Centre Expenses    {childCare(BaseScenario)}");  
        System.Console.WriteLine($"|   Less Childcare Subsidy       {subsidy(BaseScenario)}");  
        System.Console.WriteLine($"| Total Childcare Expenses       {totalCC(BaseScenario)}");  
        System.Console.WriteLine($"|");  
        System.Console.WriteLine($"| NET SURPLUS/DEFICIT            {Convert.ToInt32(netSD(BaseScenario.MyFamily.Parent1TaxableSalary, parent2TS1, BaseScenario.MyFamily.NumberOfChildren, BaseScenario))}");  
        System.Console.WriteLine($"| Net Income per Fortnight       {Convert.ToInt32(netSD(BaseScenario.MyFamily.Parent1TaxableSalary, parent2TS1, BaseScenario.MyFamily.NumberOfChildren, BaseScenario)) / 26}");  
        System.Console.WriteLine($"|---------------------------------------------------------------------------------------------------------");    

        System.Console.WriteLine($"|--------------------------------------------------------------------------------------------------------");
        System.Console.WriteLine($"| ALTERNATIVE SCENARIO SUMMARY");
        System.Console.WriteLine($"|");
        System.Console.WriteLine($"| {baseFamily.Parent1Name}");
        System.Console.WriteLine($"|   Taxable Income               {AlternativeScenario.MyFamily.Parent1TaxableSalary}");
        System.Console.WriteLine($"|   Gross Tax Payable            {GTP(AlternativeScenario.MyFamily.Parent1TaxableSalary)}");
        System.Console.WriteLine($"|   Medicare Levy                {Medicare(AlternativeScenario.MyFamily.Parent1TaxableSalary)}");
        System.Console.WriteLine($"|   Low Income Offset            {LITO(AlternativeScenario.MyFamily.Parent1TaxableSalary)}");  
        System.Console.WriteLine($"|   Mid Income Offset            {MITO(AlternativeScenario.MyFamily.Parent1TaxableSalary)}");  
        System.Console.WriteLine($"| Net Tax Payable                {nTax(AlternativeScenario.MyFamily.Parent1TaxableSalary)}");  
        System.Console.WriteLine($"|");
        System.Console.WriteLine($"| {(baseFamily as CoupleFamily).Parent2Name}");
        System.Console.WriteLine($"|   Taxable Income               {parent2TS2}");
        System.Console.WriteLine($"|   Gross Tax Payable            {GTP(parent2TS2)}");
        System.Console.WriteLine($"|   Medicare Levy                {Medicare(parent2TS2)}");     
        System.Console.WriteLine($"|   Low Income Offset            {LITO(parent2TS2)}");  
        System.Console.WriteLine($"|   Mid Income Offset            {MITO(parent2TS2)}");  
        System.Console.WriteLine($"| Net Tax Payable                {nTax(parent2TS2)}");
        System.Console.WriteLine($"|");  
        System.Console.WriteLine($"|   Family Tax A                 {Convert.ToInt32(CalculateFTBA(AlternativeScenario.MyFamily.Parent1TaxableSalary + parent2TS2, AlternativeScenario.MyFamily.NumberOfChildren))}");  
        System.Console.WriteLine($"|   Family Tax B                 {Convert.ToInt32(CalculateFTBB(AlternativeScenario.MyFamily.Parent1TaxableSalary, parent2TS2))}");  
        System.Console.WriteLine($"| Total Centrelink               {Convert.ToInt32(CalculateFTBA(AlternativeScenario.MyFamily.Parent1TaxableSalary + parent2TS2, AlternativeScenario.MyFamily.NumberOfChildren) + CalculateFTBB(AlternativeScenario.MyFamily.Parent1TaxableSalary, parent2TS2))}");  
        System.Console.WriteLine($"|");    
        System.Console.WriteLine($"| NET INCOME                     {Convert.ToInt32(calcNI(AlternativeScenario.MyFamily.Parent1TaxableSalary, parent2TS2, AlternativeScenario.MyFamily.NumberOfChildren))}");  
        System.Console.WriteLine($"|");  
        System.Console.WriteLine($"|   Childcare Centre Expenses    {childCare(AlternativeScenario)}");  
        System.Console.WriteLine($"|   Less Childcare Subsidy       {subsidy(AlternativeScenario)}");  
        System.Console.WriteLine($"| Total Childcare Expenses       {totalCC(AlternativeScenario)}");  
        System.Console.WriteLine($"|");  
        System.Console.WriteLine($"| NET SURPLUS/DEFICIT            {Convert.ToInt32(netSD(AlternativeScenario.MyFamily.Parent1TaxableSalary, parent2TS2, AlternativeScenario.MyFamily.NumberOfChildren, AlternativeScenario))}");  
        System.Console.WriteLine($"| Net Income per Fortnight       {Convert.ToInt32(netSD(AlternativeScenario.MyFamily.Parent1TaxableSalary, parent2TS2, AlternativeScenario.MyFamily.NumberOfChildren, AlternativeScenario)) / 26}");  
        System.Console.WriteLine($"|---------------------------------------------------------------------------------------------------------");    

        System.Console.WriteLine("\nBASE SCENARIO");
        BaseScenario.PrintCareDetails();
        System.Console.WriteLine($"Net income per fortnight in this scenario is ${Convert.ToInt32(netSD(BaseScenario.MyFamily.Parent1TaxableSalary, parent2TS1, BaseScenario.MyFamily.NumberOfChildren, BaseScenario)) / 26}.");
        System.Console.WriteLine($"\n---------------------------------------------------------------------------------------------------------");   
        System.Console.WriteLine("\nALTERNATE SCENARIO");
        AlternativeScenario.PrintCareDetails();
        System.Console.WriteLine($"Net income per fortnight in this scenario is ${Convert.ToInt32(netSD(AlternativeScenario.MyFamily.Parent1TaxableSalary, parent2TS2, AlternativeScenario.MyFamily.NumberOfChildren, AlternativeScenario)) / 26}.");
        System.Console.WriteLine($"\n---------------------------------------------------------------------------------------------------------");    
    }

    //declare methods

    private double GrossTaxPayable(int salary)
    {
        if(salary <= 18200){return 0;}
        else if(salary <= 37000){return (salary - 18200) * 0.19;}
        else if(salary <= 90000){return 3572 + ((salary - 37000) * 0.325);}
        else if(salary <= 180000){return 20797 + ((salary - 90000) * 0.37);}
        else if(salary > 180000){return 54097 + ((salary - 180000) * 0.45);}
        else{return 0;}
    }

    private double CalculateMedicare(int salary)
    {
        if(salary < 22399){return 0;}
        else if(salary < 27997){return (salary - 22399) * 0.1;}
        else{return salary * 0.02;}
    }

    private double CalculateLITO(int salary)
    {
        if(salary < 37000){return 445;}
        else if(salary < 66667){return 445 - ((salary - 37000)*0.015);}
        else{return 0;}
    }

    private double CalculateMITO(int salary)
    {
        if(salary < 37000){ return 255; }
        else if(salary < 48000){return 255 + ((salary - 37000)*0.075);}
        else if (salary < 90000){return 1080;}
        else if(salary < 126000){return 1080 - ((salary - 90000)*0.03);}
        else{return 0;}
    }
    
    private double CalculateFTBA(int familyIncome, int children)
    {
        if(familyIncome <= 54677)
        {
            return children * (4855 + 767);
        } 
        else if(familyIncome <= 80000)
        {
            double reduction = (familyIncome - 54677) * 0.2; 
            double reducedRate = 4855 - reduction;
            return (reducedRate + 767) * children;
        }
        else if(familyIncome <= 98988)
        {
            double reduction = (familyIncome - 54677) * 0.2; 
            double reducedRate = 4855 - reduction;
            return reducedRate * children;
        }
        else if(familyIncome <= 115000)
        {
            double reduction = (familyIncome - 98988) * 0.3;
            double reducedRate = 4855 - reduction;
            return reducedRate * children;
        }
        else{return 0;}       
    }

    private double CalculateFTBB(int p1Salary)
    {
        if(p1Salary < 100000)
        {
            if(p1Salary < 5694){return (4128 + 372);}
            else if(p1Salary < 28197)
            {
                double reduction = (p1Salary - 5694) * 0.2;
                double reducedRate = 4128 + 372 - reduction;
                return reducedRate;
            }
            else {return 0;}
        }
        else {return 0;}
    }

    private double CalculateFTBB(int p1Salary, int p2Salary)
    {
        if(p1Salary > p2Salary && p1Salary < 100000)
        {
            if(p2Salary < 5694){return (4128 + 372);}
            else if(p2Salary < 28197)
            {
                double reduction = (p2Salary - 5694) * 0.2;
                double reducedRate = 4128 + 372 - reduction;
                return reducedRate;
            }
            else {return 0;}
        }
        else if(p1Salary < p2Salary && p2Salary < 100000)
        {
            if(p1Salary < 5694){return (4128 + 372);}
            else if(p1Salary < 28197)
            {
                double reduction = (p1Salary - 5694) * 0.2;
                double reducedRate = 4128 + 372 - reduction;
                return reducedRate;
            }
            else{return 0;}
        }
        else {return 0;}
    }
}