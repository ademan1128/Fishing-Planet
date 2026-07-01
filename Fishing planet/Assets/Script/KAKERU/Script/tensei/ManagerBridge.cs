using UnityEngine;
public class Bridge : MonoBehaviour { void Awake() { DontDestroyOnLoad(gameObject); } }