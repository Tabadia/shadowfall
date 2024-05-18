using NUnit.Framework;
using UnityEngine;

public class GameObjectTest
{
    [Test]
    public void GameObject_NewGameObject_IsNotActive()
    {
        // Arrange
        GameObject gameObject = new GameObject();

        // Assert
        Assert.IsFalse(gameObject.activeSelf);
    }

    [Test]
    public void GameObject_ActivateGameObject_IsActive()
    {
        // Arrange
        GameObject gameObject = new GameObject();

        // Act
        gameObject.SetActive(true);

        // Assert
        Assert.IsTrue(gameObject.activeSelf);
    }
}