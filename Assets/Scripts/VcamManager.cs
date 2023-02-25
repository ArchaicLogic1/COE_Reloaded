using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class VcamManager : MonoBehaviour
{
    public static VcamManager instance;
    // Start is called before the first frame update
    [SerializeField] int moveDirRef=0;
    [SerializeField] GameObject vCamLookRight, vCamLookLeft, vCamLookCenter,vcamBossFight;
    CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin;
    CinemachineVirtualCamera vCamLeft, vCamRight, vCamCenter;
    public bool bossFight;
    public GameObject myCurrentVcam;
    // Update is called once per frame
    void Start()
    {
        instance = this;
    //  DontDestroyOnLoad(gameObject);
    //      if(instance== null)
    //       {
    //          instance = this;
    //       }
    //  else
    //  {
    //      Destroy(gameObject);
    //  }
        
        vCamLeft = vCamLookLeft.GetComponent<CinemachineVirtualCamera>();
        vCamRight = vCamLookRight.GetComponent<CinemachineVirtualCamera>();
        vCamCenter = vCamLookCenter.GetComponent<CinemachineVirtualCamera>();
        //moveDirRef= PlayerController.moveDir;
       swapVcam();
        vCamLeft.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0;
        vCamRight.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0;
        vCamCenter.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0;

    }
    void Update()
    {
        moveDirRef = PlayerController.instance.moveDir;

        swapVcam();
       
    }
   

    void swapVcam()
    {
        if (!bossFight)
        {
            if (moveDirRef == 1 && PlayerController.isGrounded)
            {
                vCamLookRight.SetActive(true);
                myCurrentVcam = vCamLookRight;

                // turnoff below
                vCamLookCenter.SetActive(false);
                vCamLookLeft.SetActive(false);
            }
            else if (moveDirRef == -1 && PlayerController.isGrounded)
            {
                vCamLookLeft.SetActive(true);
                myCurrentVcam = vCamLookLeft;
                // turnoff below
                vCamLookCenter.SetActive(false);
                vCamLookRight.SetActive(false);
            }
            else if (!PlayerController.isGrounded)
            {
                vCamLookCenter.SetActive(true);
                myCurrentVcam = vCamLookCenter;

                // turnoff below
                vCamLookLeft.SetActive(false);
                vCamLookRight.SetActive(false);

            }
        }
        if(bossFight)
        {
            vcamBossFight.SetActive(true);
            myCurrentVcam = vcamBossFight;
            vCamLookCenter.SetActive(false);
            vCamLookLeft.SetActive(false);
            vCamLookRight.SetActive(false);
        }
        
       
    //  vCamLookRight.SetActive(moveDirRef == 1 ? true : false);
    //  vCamLookLeft.SetActive(moveDirRef == -1 ? true : false);
    }
    public void CamShake(float intensity, float time)
    {
        Debug.Log(myCurrentVcam);
        CinemachineVirtualCamera _currentVcam = myCurrentVcam.GetComponent<CinemachineVirtualCamera>();
        Debug.Log(_currentVcam);
        
    cinemachineBasicMultiChannelPerlin = _currentVcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        Debug.Log(cinemachineBasicMultiChannelPerlin);

        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;


         instance.StartCoroutine(instance.shake(time));

    }
   
    public IEnumerator shake(float time)
    { 
        yield return new WaitForSeconds(time);
        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0;

    }
    public void DirswapCamChange()
    {
       
    }
    public void setBossFightCamActive()
    {
        bossFight = true;
    }
}
