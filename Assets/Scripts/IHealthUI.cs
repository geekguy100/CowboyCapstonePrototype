/*****************************************************************************
// File Name :         IHealthUI.cs
// Author :            Kyle Grenier
// Creation Date :     #CREATIONDATE#
//
// Brief Description : Contract for creating UI for a Health component.
*****************************************************************************/
public interface IHealthUI
{
    void DecreaseHealth(int amnt);
    void IncreaseHealth(int amnt);
}
