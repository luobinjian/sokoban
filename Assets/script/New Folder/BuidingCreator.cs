using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;

public class BuidingCreator : Singleton<BuidingCreator>
{
    [SerializeField] Tilemap PriviewTilemap;
    [SerializeField] Tilemap DefaultTilemap;
    [SerializeField] Tilemap BoxTilemap;
    [SerializeField] Tilemap PlayerTilemap;
    [SerializeField] Tilemap TargetTilemap;
    [SerializeField] Tilemap iceTilemap;
    [SerializeField] Tilemap groundTilemap;

    TileBase tileBase;
    Camera _camera;

    Vector3Int currentGridPos;
    Vector3Int lastGridPos;
   PlayerInput player;
    BuildingObjectBase selectedObj;
   
   Vector2 mousePos;
   

   protected override void Awake()
   {
       base.Awake();
       player = new PlayerInput();
       _camera = Camera.main;
   }

   private void OnEnable()
   {
       player.Enable();

       player.Newactionmap.MousePosition.performed += onMouseMove;
       player.Newactionmap.MouseLeft.performed += onLeftClick;
       player.Newactionmap.MouseRight.performed += onRightClick;
   }

   private void OnDisable()
   {
       if (player != null)
       {
           player.Disable();

           player.Newactionmap.MousePosition.performed -= onMouseMove;
           player.Newactionmap.MouseLeft.performed -= onLeftClick;
           player.Newactionmap.MouseRight.performed -= onRightClick;
       }
   }

   private BuildingObjectBase SelectedObj
   {
       set{
           selectedObj = value;

           tileBase = selectedObj != null? selectedObj.TileBase : null;

           UpdatePreview();
       }
   }
    private void Update()
    {
        if (selectedObj != null)
        {
            Vector3 pos = _camera.ScreenToWorldPoint(mousePos);
            Vector3Int gridPos = PriviewTilemap.WorldToCell(pos);

            if (gridPos != currentGridPos)
            {
                lastGridPos = currentGridPos;
                currentGridPos = gridPos;
                UpdatePreview();
            }
        }
    }

    private void onMouseMove(InputAction.CallbackContext context)
    {
        mousePos = context.ReadValue<Vector2>();
    }

    private void onLeftClick(InputAction.CallbackContext context)
    {
        if(selectedObj != null && !EventSystem.current.IsPointerOverGameObject())
        {
        HandleDrawing();
        }
    }

    private void onRightClick(InputAction.CallbackContext context)
    {
        SelectedObj = null;
    }

    public void ObjectSelected(BuildingObjectBase obj)
    {
        SelectedObj = obj;
    }

    private void UpdatePreview()
    {
        PriviewTilemap.SetTile(lastGridPos, null);
        PriviewTilemap.SetTile(currentGridPos, tileBase);
    }

    private void HandleDrawing()
    {
        if (selectedObj != null && selectedObj.Type == Type.Wall)
        {
            DefaultTilemap.SetTile(currentGridPos, tileBase);
        }
        else if (selectedObj != null && selectedObj.Type == Type.box)
        {
            BoxTilemap.SetTile(currentGridPos, tileBase);
        }
        else if (selectedObj != null && selectedObj.Type == Type.player)
        {
            PlayerTilemap.SetTile(currentGridPos, tileBase);
        }
        else if (selectedObj != null && selectedObj.Type == Type.target)
        {
            TargetTilemap.SetTile(currentGridPos, tileBase);
        }
        else if (selectedObj != null && selectedObj.Type == Type.ice)
        {
            iceTilemap.SetTile(currentGridPos, tileBase);
        }
        else if (selectedObj != null && selectedObj.Type == Type.ground)
        {
            groundTilemap.SetTile(currentGridPos, tileBase);
        }
    }

}
