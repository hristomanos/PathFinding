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
    

    private TextMeshProUGUI m_Text;
    private TileType g_TileType;

    private SpriteRenderer m_Renderer;
    private int _x;
    private int _y;


    void Awake()
    {
        m_Text = GetComponentInChildren<TextMeshProUGUI>();
        m_Renderer = GetComponent<SpriteRenderer>();
    }

    public void Init(int x, int y)
    {
        _x = x;
        _y = y;
        name = "Tile_" + x + "_" + y;
    }

    public Color g_Color { get => m_Renderer.material.color; set => m_Renderer.material.color = value; }
    public string g_Text { get => m_Text.text; set => m_Text.text = value; }
    public TileType _TileType
    {
        get => g_TileType;
        set
        {
            g_TileType = value;
            switch (g_TileType)
            {
                case TileType.Plains:
                    m_Renderer.material.color = Color.white;
                    
                    break;
                case TileType.Wall:
                    g_Color = Color.gray;
                    break;
                case TileType.Wood:
                    m_Renderer.material.color = Color.green;
                    break;
            }
        }
    }

    public int g_Cost
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
