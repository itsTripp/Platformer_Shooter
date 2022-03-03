using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorParamSetter : MonoBehaviour
{
    Animator _animator;

    private void Start()
    {
        string missingComponents = "";
        if (!TryGetComponent(out _animator))
        {
            missingComponents += "Animator,";
            if (missingComponents.Length > 0)
            {
                missingComponents = missingComponents.Trim(','); //Trim the last comma off.
                Debug.LogError(this.gameObject.name + " is missing the follow required components for its 'AnimatorParamSetter' component (" + missingComponents + ")");
            }
        }
    }

    public void SetBool(string parameterString)
    {
        string[] parameter = parameterString.Split('|');
        string parameterName = parameter[0];
        bool value;

        if (bool.TryParse(parameter[1], out value))
        {
            _animator.SetBool(parameterName, value);
        } else
        {
            Debug.LogError("SetBool called with invalid parameterString");
        }
        
    }

    public void SetFloat(string parameterString)
    {
        string[] parameter = parameterString.Split('|');
        string parameterName = parameter[0];
        float value;

        if (float.TryParse(parameter[1], out value))
        {
            _animator.SetFloat(parameterName, value);
        }
        else
        {
            Debug.LogError("SetFloat called with invalid parameterString");
        }
    }

    public void SetInt(string parameterString)
    {
        string[] parameter = parameterString.Split('|');
        string parameterName = parameter[0];
        int value;

        if (int.TryParse(parameter[1], out value))
        {
            _animator.SetInteger(parameterName, value);
        }
        else
        {
            Debug.LogError("SetInt called with invalid parameterString");
        }
    }

    public void SetTrigger(string triggerName)
    {
        _animator.SetTrigger(triggerName);
    }
}
