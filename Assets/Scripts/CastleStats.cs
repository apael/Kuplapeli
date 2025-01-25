using UnityEngine;

public class CastleStats : UnitBaseStats
{
    protected override void Start()
    {
        // Customize stats for Unit 2
        
        maxHealth = 5000;
        

        // Call the base class's Start method
        base.Start();
    }
}
