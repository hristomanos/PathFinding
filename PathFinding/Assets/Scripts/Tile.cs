using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Tile : MonoBehaviour
{
    public enum TileType
    {
        Plains,
        Wall,
        Wood
    }

   // public GameObject _woodGO;
   // public GameObject _wallGO;
    

    private TextMeshProUGUI _text;
    private TileType g_TileType;

    private SpriteRenderer _renderer;
    private int _x;
    private int _y;


    void Awake()
    {
        _text = GetComponentInChildren<TextMeshProUGUI>();
        _renderer = GetComponent<SpriteRenderer>();
    }

    public void Init(int x, int y)
    {
        _x = x;
        _y = y;
        name = "Tile_" + x + "_" + y;
    }

    public Color _Color { get => _renderer.material.color; set => _renderer.material.color = value; }
    public string _Text { get => _text.text; set => _text.text = value; }
    public TileType _TileType
    {
        get => g_TileType;
        set
        {
            g_TileType = value;
            switch (g_TileType)
            {
                case TileType.Plains:
                    _renderer.material.color = Color.white;
                    
                    break;
                case TileType.Wall:
                    _Color = Color.gray;
                    break;
                case TileType.Wood:
                    _renderer.material.color = Color.green;
                    break;
            }
        }
    }
    public int _Cost
    {
        get
        {
            switch (g_TileType)
            {
                case TileType.Plains:
                    return 1;
                case TileType.Wood:
                    return 5;
                default:
                    return 0;
            }
        }
    }
    public int _X => _x; 
    public int _Y => _y;
}
