using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SphereController : MonoBehaviour
{
    [SerializeField]
    private List<Material> _materials;
    [SerializeField]
    private GameObject _sphere;
    [SerializeField]
    private RaycastSphere _raycastSphere;
    [SerializeField]
    private List<GameObject> _contact;

    private Vector3 _randomPos;

    public bool isEnabled = false;


    [SerializeField]
    private Camera _camera;

    private Collider collid;
    public void PlaceSphere()
    {
        _sphere.GetComponent<Renderer>().material = _materials[Random.Range(0, _materials.Count)];
        _contact = _raycastSphere.GetTarget();
              
        if (_contact.Count > 0)
        {
            int random = Random.Range(0, _contact.Count);
            float distanceToTarget = Vector3.Distance(_contact[random].transform.position, _camera.transform.position);
            collid = _contact[random].gameObject.GetComponent<Collider>();
            _randomPos = collid.bounds.center + Random.insideUnitSphere * collid.bounds.extents.magnitude;
            _randomPos =  collid.ClosestPoint(_randomPos);
            _sphere.transform.position = new Vector3(_randomPos.x, _randomPos.y, _randomPos.z - distanceToTarget / 10);
            _sphere.gameObject.SetActive(true);
            isEnabled = true;

        }
        
    }
    public void DisableSphere()
    {
        _sphere.gameObject.SetActive(false);
        isEnabled = false;
    }
    public void ScaleSphere()
    {
        _sphere.transform.localScale -= new Vector3(0.01f, 0.01f, 0.01f);
    }
    public void ResetScale()
    {
        _sphere.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
    }
    public IEnumerator LaunchSphere()
    {
        while (isEnabled)
        {
            Debug.Log("Bonjour je suis olivier de chez carglass");
            _sphere.transform.position = Vector3.Lerp(_sphere.transform.position, new Vector3(_sphere.transform.position.x, _sphere.transform.position.y, _randomPos.z), 0.05f);
            yield return null;
        }
        
    }
}