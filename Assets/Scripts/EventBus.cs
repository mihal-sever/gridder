using System;
using UnityEngine;

public static class EventBus
{
    public static event Action<Sprite> ImageChosen;
    public static event Action ImageSetup;
    public static event Action UserInputValidated;
    public static event Action<ProjectDto> ProjectLoaded;


    public static void OnImageChosen(Sprite sprite)
    {
        ImageChosen?.Invoke(sprite);
    }

    public static void OnImageSetup()
    {
        ImageSetup?.Invoke();
    }
    
    public static void OnUserInputValidated()
    {
        UserInputValidated?.Invoke();
    }

    public static void OnProjectLoaded(ProjectDto projectDto)
    {
        ProjectLoaded?.Invoke(projectDto);
    }
}