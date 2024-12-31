using UnityEngine;
using System.Collections.Generic;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.InputSystem;

[RequireComponent(typeof(ARRaycastManager))]
public class PlaceOnPlane : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The prefab to instantiate when the user taps on a plane.")]
    GameObject m_PlacedPrefab;

    /// <summary>
    /// The prefab to instantiate on touch.
    /// </summary>
    public GameObject placedPrefab
    {
        get { return m_PlacedPrefab; }
        set { m_PlacedPrefab = value; }
    }

    /// <summary>
    /// The object instantiated as a result of a successful raycast intersection with a plane.
    /// </summary>
    public GameObject spawnedObject { get; private set; }

    ARRaycastManager m_RaycastManager;
    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();


    void Awake()
    {
        m_RaycastManager = GetComponent<ARRaycastManager>();
    }

    void Update()
    {
        // Wenn der Bildschirm berührt wird
        if (Touchscreen.current.primaryTouch.press.isPressed)
        {
            var touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();

            // Raycast auf die Ebene, wenn die Berührung erkannt wurde
            if (m_RaycastManager.Raycast(touchPosition, s_Hits, TrackableType.PlaneWithinPolygon))
            {
                // Das erste Trefferziel (nächste Ebene) wird verwendet
                var hitPose = s_Hits[0].pose;

                // Objekt nur einmal instanziieren, wenn es noch nicht existiert
                if (spawnedObject == null)
                {
                    spawnedObject = Instantiate(m_PlacedPrefab, hitPose.position, hitPose.rotation);
                }
                else
                {
                    // Wenn das Objekt bereits existiert, nur die Position aktualisieren
                    spawnedObject.transform.position = hitPose.position;
                }
            }
        }
    }
}
