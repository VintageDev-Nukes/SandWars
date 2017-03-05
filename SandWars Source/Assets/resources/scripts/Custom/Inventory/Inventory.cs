using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public enum ItemTypes {None, Gun, Ammo, Weapon, Money, Equip, Accesory, Bag, Consume, Stack}
public enum EquipmentType {None, Helmet, Chest, Leggings, Boots, Hat, Googles, Backpack, Coat, Shirt, Belt}
public enum UseTypes {None, Use, Consume, Stack}

public class InventoryItem
{
	public GameObject worldObject; //This is the item's object what will be in the world
	public int id; //Item's ID (Identificator number)
	public string itemname; //Item's name
	public string DisplayName; //Description
	public Texture2D itemtex; //Texture in inventory
	public ItemTypes itemtype = ItemTypes.None; //Gun, ammo, weapon, money, equip, accesories.
	public EquipmentType equipmenttype = EquipmentType.None; //Slots, Inventory, Ammo, Accesories ---... New use: if the itemtype is armor or equip (is the same) set the subtype (Head (Mask || Heltmet), Googles, Chest, Shirt, Trousers || Pants, Leggings, Boots) (Armor || Clothes)
	//This can be used for example for slot quickly
	//Use also weapons as decoration, that's we can see in all Metal Assaults' Invetory Screenshots
	//public string usable; //Is usable? (I don't know if it means that can be used for health you like a potion?)
	public bool isUsable; //Can be used or this item is blocked
	public UseTypes usable = UseTypes.None; //This is type of use...
	public float itemweight; //Weight in inventory, maybe for make an weightable inventory?
	public bool droppable = true; //Can be drop?
	
	public int itemstacksize = 1; //Current stack number
	public int itemstacklimit = 1; //Max number nack
	public bool showStack = true; //Show stack in GUI?

	//This is for make a bag, maybe it will come inside of the features of the game
	public int bagsize;
	public bool showBag;
	public InventoryItem[] BagItem;

	public Vector3 CustomPostion;
	public Vector3 CustomRotation;
	public Vector3 CustomScale;
	
}

public class ItemBase {
	
	private static InventoryItem[] _itemBase;
	
	public static InventoryItem[] MainBase {
		get {return _itemBase;}
		set {_itemBase = value;}
	}
	
}

public class Slots {
	
	private static InventoryItem[] _HBSlots;
	private static InventoryItem[] _InvSlots;
	//private static InventoryItem[] _ASlots;
	private static InventoryItem[] _AccSlots;
	private static InventoryItem[] _EquipedItem;
	
	private static InventoryItem[,] _WorldChests;
	
	public static InventoryItem[] HotBarSlots {
		get {return _HBSlots;}
		set {_HBSlots = value;}
	}
	
	public static InventoryItem[] InventorySlots {
		get {return _InvSlots;}
		set {_InvSlots = value;}
	}
	
	/*public static InventoryItem[] AmmoSlots {
		get {return _ASlots;}
		set {_ASlots = value;}
	}*/
	
	public static InventoryItem[] Accesories {
		get {return _AccSlots;}
		set {_AccSlots = value;}
	}
	
	public static InventoryItem[] EquipedItem {
		get {return _EquipedItem;}
		set {_EquipedItem = value;}
	}
	
	public static InventoryItem[,] WorldChests {
		get {return _WorldChests;}
		set {_WorldChests = value;}
	}
	
	private static Rect _HBSlotsRect;
	private static Rect _InvSlotsRect;
	//private static Rect _ASlotsRect;
	private static Rect _AccSlotsRect;
	
	public static Rect HotBarSlotsRect {
		get {return _HBSlotsRect;}
		set {_HBSlotsRect = value;}
	}
	
	public static Rect InventorySlotsRect {
		get {return _InvSlotsRect;}
		set {_InvSlotsRect = value;}
	}
	
	/*public static Rect AmmoSlotsRect {
		get {return _ASlotsRect;}
		set {_ASlotsRect = value;}
	}*/
	
	public static Rect AccesoriesRect {
		get {return _AccSlotsRect;}
		set {_AccSlotsRect = value;}
	}
	
}

public class SelectedSlot {
	
	private static int _SSlot;
	private static GameObject _carriedObj;
	
	public static int SSlot {
		get {return _SSlot;}
		set {_SSlot = value;}
	}
	
	public static GameObject CarriedObject {
		get {return _carriedObj;}
		set {_carriedObj = value;}
	}
	
	public static string cName;
	public static int lastSlot;
	public static bool switchedPer;
	
}

public class RegItem {
	
	private static float _itemCount = 0;
	
	public static float itemCount {
		get {return _itemCount;}
		set {_itemCount = value;}
	}
	
}

public class Inv {
	
	//Maximum items possible
	private int MaxItemBase = 256;
	
	private InventoryItem[] localItemBase;
	
	public InventoryItem FindItem(int id) {
		
		InventoryItem returnvalue = null;
		
		try {
			returnvalue = ItemBase.MainBase[id];
		} catch(System.NullReferenceException e) {
			Debug.Log(e+"; ID = "+id);
		}
		
		return returnvalue;
		
	}
	
	/*private GameObject LoadItemPrefab(string name) {
		return (GameObject)Resources.Load("prefabs/Items/"+name);
	}
	
	private Texture LoadItemTex(string name) {
		return (Texture)Resources.Load("textures/Items/"+name);
	}
	
	private GameObject LoadItemModel(string name) {
		return (GameObject)Resources.Load ("models/Items/"+name);
	}*/

	private GameObject LoadWorldObject(string name) {
		return Resources.Load<GameObject>("worldObjects/Items/"+name);
	}
	private Texture2D LoadWOTextures(string name) {
		return Resources.Load<Texture2D>("worldObjects/Items/Textures/"+name);
	}
	
	private InventoryItem[] Load() {
		
		localItemBase = new InventoryItem[MaxItemBase];
		
		InventoryItem preset;
		
		preset = new InventoryItem() {id = 1, itemname = "test", DisplayName = "Test", itemtex = LoadWOTextures("test"), worldObject = LoadWorldObject("test")};

		localItemBase[preset.id] = preset;
		
		return localItemBase;
		
	}
	
	
	//This makes the script a global script so that anyone can access it
	//public static Inv statInventory;
	InventoryItem[] bankinventory;
	
	//This is to check if the mouse is over the item to display the info, it takes the tooltip and compares it
	string mouseOver; 
	
	//This is the 8 slot main inventory
	//InventoryItem[] inventory;
	//This is the equiped items inventory
	//InventoryItem[] Slots.EquipedItem;
	
	//This is the money array
	//InventoryItem[] inventorymoney;
	
	public Texture emptyTex;
	Texture bagtexture;
	int iconSizeW = 37;
	int iconSizeH = 39;
	
	//This is the object the mouse is carrying
	InventoryItem mouseitem = null;
	
	//This is the item being displayed in the tooltip
	private InventoryItem tooltipitem1 = null;
	private InventoryItem tooltipitem2 = null;
	private InventoryItem tooltipitem3 = null;
	private float tooltipx1 = 0.0f;
	private float tooltipy1 = 0.0f;
	private float tooltipx2 = 0.0f;
	private float tooltipy2 = 0.0f;
	private float tooltipx3 = 0.0f;
	private float tooltipy3 = 0.0f;
	
	int stackamount = 1;
	private bool readytodrop = false;
	bool mouseheld = true;
	
	string togglefullscreen = "Off";
	float totalweight = 0;	
	
	private bool itemsLoaded = false;
	private bool gameInvLoaded = false;
	
	private Transform player;
	private GameObject playerObject;
	
	//Principal variable of Inventories, esta variable se asigna mas tarde segun el tipo de Item
	
	private InventoryItem[] inventory;
	
	private InventoryItem[] inv;
	
	public bool SceneItemToInv(int id) {
		return AddItem(FindItem(id));
	}
	
	public InventoryItem GetItem(InventoryItem[] inventory, int slot) {
		return inventory[slot];
	}
	
	private int SlotSetted;
	
	private bool slotsLoaded = false;
	
	public void ChangeItemName(int id, string newname) {
		ItemBase.MainBase[id].DisplayName = newname;
	}
	
	private readonly string[] SlotsTypes = new string[] {"Accesories", "HotBar", "Ammo", "Inventory", "Equip"};
	
	public bool AddItem(InventoryItem item, int position = -1, string fp = "") {

		ItemTypes findPath = (ItemTypes)System.Enum.Parse(typeof(ItemTypes), fp);

		//findPath: Obtiene el tipo del item y busca el sitio mas idoneo para meterlo. (String.Empty: Si, Else: enum.parse)
		
		bool autoDetect = System.String.IsNullOrEmpty(fp) || System.Enum.GetNames(typeof(ItemTypes)).Any(x=> x != fp);
		
		if(autoDetect) {
			switch(item.itemtype) {
				
			case ItemTypes.Accesory:
				for(int x=0; x<Slots.Accesories.Length;x++) {
					if(Slots.Accesories[x] == null) {
						Slots.Accesories[x] = item;
						return true;
					}
					
					if(x == Slots.Accesories.Length) {
						return false;
					}
				}
				break;
				
			case ItemTypes.Equip:
				for(int x=0; x<Slots.EquipedItem.Length;x++) {
					if(Slots.EquipedItem[x] == null) {
						Slots.EquipedItem[x] = item;
						return true;
					}
					
					if(x == Slots.EquipedItem.Length) {
						return false;
					}
				}
				break;
				
			/*case "Ammo":
				for(int x=0; x<Slots.AmmoSlots.Length;x++) {
					if(Slots.AmmoSlots[x] == null) {
						Slots.AmmoSlots[x] = item;
						return true;
					}
					
					if(x == Slots.AmmoSlots.Length) {
						return false;
					}
				}
				break;*/
				
			default:
				
				if(position < 0) {
					
					//Detectar si el inventario esta lleno para asignar los HotBarslots o los InventorySlots, y si esta lleno del todo salir del sub...
					
					//If autodetect, determinar el ultimo caso como el invetario (default case), es decir el actual, y mientras hacer switches 'case itemType: poner en un slot del tipo deseado'
					
					for(int x=0; x<Slots.InventorySlots.Length;x++) {
						if(Slots.InventorySlots[x] == null) {
							Slots.InventorySlots[SlotSetted] = item;
							return true;
						}
						
					}

					for(int x=0;x<Slots.HotBarSlots.Length;x++) {
						if(Slots.HotBarSlots[x] == null) {
							Slots.HotBarSlots[SlotSetted] = item;
							return true;
						}
						
						if(x == Slots.HotBarSlots.Length) {
							return false;
						}
					}
					
				} else {
					try {
						Slots.InventorySlots[position] = item;
					} catch(System.NullReferenceException e) {
						//This means that inventory is full
						return false;
					}
				}
				break;
				
			}
		} else {
			
			//private readonly string[] SlotsTypes = new string[] {"Accesories", "HotBar", "Ammo", "Inventory", "Equip"};
			
			if(position < 0) {
				
				if(findPath == ItemTypes.Accesory) {
					
					for(int x=0; x<Slots.Accesories.Length;x++) {
						if(Slots.Accesories[x] == null) {
							Slots.Accesories[x] = item;
							return true;
						}
						
						if(x == Slots.Accesories.Length) {
							return false;
						}
					}
					
				} /*else if(findPath == SlotsTypes[1]) {
					
					for(int x=0;x<Slots.HotBarSlots.Length;x++) {
						if(Slots.HotBarSlots[x] == null) {
							Slots.HotBarSlots[x] = item;
							return true;
						}
						
						if(x == Slots.HotBarSlots.Length) {
							return false;
						}
					}
					
				}*/ //else if(findPath == ItemTypes.Ammo]) {
					
					/*for(int x=0; x<Slots.AmmoSlots.Length;x++) {
						if(Slots.AmmoSlots[x] == null) {
							Slots.AmmoSlots[x] = item;
							return true;
						}
						
						if(x == Slots.AmmoSlots.Length) {
							return false;
						}
					}*/
					
				//} 
				else if(findPath == ItemTypes.None) {
					
					for(int x=0; x<Slots.InventorySlots.Length;x++) {
						if(Slots.InventorySlots[x] == null) {
							Slots.InventorySlots[x] = item;
							return true;
						}
						
						if(x == Slots.InventorySlots.Length) {
							return false;
						}
						//inventory = InventorySlots;
					}
					
				} else if(findPath == ItemTypes.Equip) {
					
					for(int x=0; x<Slots.EquipedItem.Length;x++) {
						if(Slots.EquipedItem[x] == null) {
							Slots.EquipedItem[x] = item;
							return true;
						}
						
						if(x == Slots.EquipedItem.Length) {
							return false;
						}
					}
					
				}
				
			} else {
				
				if(findPath == ItemTypes.Accesory) {
					try {
						Slots.Accesories[position] = item;
						return true;
					} catch {
						return false;
					}
				} /*else if(findPath == SlotsTypes[1]) {
					try {
						Slots.HotBarSlots[position] = item;
						return true;
					} catch {
						return false;
					}
				} else if(findPath == SlotsTypes[2]) {
					try {
						Slots.AmmoSlots[position] = item;
						return true;
					} catch {
						return false;
					}
				}*/ else if(findPath == ItemTypes.None) {
					try {
						Slots.InventorySlots[position] = item;
						return true;
					} catch {
						return false;
					}
				} else if(findPath == ItemTypes.Equip) {
					try {
						Slots.EquipedItem[position] = item;
						return true;
					} catch {
						return false;
					}
				}
				
			}
			
		}
		
		return false;
		
		/*if(position < 0) {

			//Detectar si el inventario esta lleno para asignar los HotBarslots o los InventorySlots, y si esta lleno del todo salir del sub...

			//If autodetect, determinar el ultimo caso como el invetario (default case), es decir el actual, y mientras hacer switches 'case itemType: poner en un slot del tipo deseado'

			for(int x=0; x<Slots.InventorySlots.Length;x++) {
				if(Slots.InventorySlots[x] == null) {
					inventory = Slots.InventorySlots;
					SlotSetted = x;
					break;
				}
				
				if(x == Slots.InventorySlots.Length) {
					InventoryIsFull = true;
				}
				//inventory = InventorySlots;
			}
			
			if (InventoryIsFull) {
				for(int x=0;x<Slots.HotBarSlots.Length;x++) {
					if(Slots.HotBarSlots[x] == null) {
						inventory = Slots.HotBarSlots;
						SlotSetted = x;
						//GameObject.DestroyImmediate(item.worldObject, true);
						break;
					}
					
					if(x == Slots.HotBarSlots.Length) {
						AllIsFull = true;
					}
				}
			}
			
			if (AllIsFull) {
				return;		
			}

			if (!InventoryIsFull) {
				Slots.InventorySlots[SlotSetted] = item;		
			} else {
				Slots.HotBarSlots[SlotSetted] = item;
			}

		} else {
			try {
				Slots.InventorySlots[position] = item;
			} catch(System.NullReferenceException e) {
				Debug.Log(e+"; InvItem = "+item);
			}
		}*/
		
	} 
	
	
	public void newStack(int slot, int stackamount, InventoryItem[] arraypassed)
	{
		
		int itemid;
		
		itemid = arraypassed[slot].id;
		
		mouseitem = FindItem(itemid);
		
	}
	
	private bool setnewItem = false;
	
	public void inventoryItemSlot (InventoryItem[] arraypassed, int slot, float xpos, float ypos){
		
		string tooltipid = "";
		
		if(arraypassed == Slots.InventorySlots) {
			tooltipid = "Invs_";
		} else if(arraypassed == Slots.Accesories) {
			tooltipid = "Accs_";
		} /*else if(arraypassed == Slots.AmmoSlots) {
			tooltipid = "Ammo_";
		}*/ else if(arraypassed == Slots.HotBarSlots) {
			tooltipid = "HotB_";
		}
		
		tooltipid = tooltipid + slot;
		if (arraypassed[slot] != null && mouseitem == null) {
			if (ItemBox(xpos, ypos, "", tooltipid)) {
				if (Input.GetKeyUp(KeyCode.Mouse0)) {
					if(arraypassed[slot].itemtype == ItemTypes.Bag){
						arraypassed[slot].showBag = false;
					}
					stackCheck(arraypassed);
					mouseitem = arraypassed[slot];
					arraypassed[slot] = null;
				}
				if (Input.GetKeyUp(KeyCode.Mouse1)){
					if (arraypassed[slot]!= null && arraypassed[slot].itemtype == ItemTypes.Stack){
						if (Input.GetKey(KeyCode.RightShift) || Input.GetKey(KeyCode.LeftShift)){
							//DO SOMETHING IF SHIFT LEFT CLICK HAPPENS
							//need to check if there is alredy a tooltip for amount picker if so then we can use any other items put if statements
							stackamount = 1;
							stackCheck(arraypassed);
							arraypassed[slot].showStack = true;
						}
					}
					if (arraypassed[slot]!= null && arraypassed[slot].itemtype == ItemTypes.Bag){
						if (Input.GetKey(KeyCode.RightShift) || Input.GetKey(KeyCode.LeftShift)){
							openAllBags(arraypassed);
						} else {
							if (arraypassed[slot].showBag){
								arraypassed[slot].showBag = false;
							} else {
								arraypassed[slot].showBag = true;
							}
						}
					}
					if (arraypassed[slot]!= null && arraypassed[slot].itemtype == ItemTypes.Equip){
						if (Input.GetKey(KeyCode.RightShift) || Input.GetKey(KeyCode.LeftShift)){
							rightClickEquip(arraypassed,slot,true);
						} else {
							rightClickEquip(arraypassed,slot,false);
						}
					}
					if (arraypassed[slot] != null && arraypassed[slot].usable == UseTypes.Consume){
						if (arraypassed[slot].itemtype != ItemTypes.Stack){
							//check timer
							Object.Destroy(arraypassed[slot].worldObject);
							arraypassed[slot] = null;
							//start timer
						} else {
							if (arraypassed[slot].itemstacksize == 1){
								Object.Destroy(arraypassed[slot].worldObject);
								arraypassed[slot] = null;
								//start timer
							} else {
								arraypassed[slot].itemstacksize = arraypassed[slot].itemstacksize - 1;
								//start timer
							}
						}
					}
					if (arraypassed[slot] != null && arraypassed[slot].usable == UseTypes.Stack){
						if (arraypassed[slot].itemtype != ItemTypes.Equip){
							//do effect
							Debug.Log ("DO EFFECT");
							//start timer
						}
					}
				}
			}
		} else {
			if (ItemBox(xpos, ypos, "", tooltipid)) {
				if (Input.GetKeyUp(KeyCode.Mouse0)) {
					if (mouseitem != null) {
						if (arraypassed[slot] != null) {
							if (arraypassed[slot].itemtype == ItemTypes.Bag){
								arraypassed[slot].showBag = false;
							}
							if (arraypassed[slot].itemtype == ItemTypes.Stack){
								arraypassed[slot].showStack = false;
							}
						}
						if (arraypassed[slot] != null && arraypassed[slot].itemtype == ItemTypes.Stack && mouseitem.itemtype == ItemTypes.Stack && arraypassed[slot].itemname == mouseitem.itemname ){//&& check if they are both the same type of stack else it
							
							if (arraypassed[slot].itemstacksize < arraypassed[slot].itemstacklimit){
								arraypassed[slot].itemstacksize = arraypassed[slot].itemstacksize + mouseitem.itemstacksize;
								if (arraypassed[slot].itemstacksize > arraypassed[slot].itemstacklimit){
									mouseitem.itemstacksize = arraypassed[slot].itemstacksize - arraypassed[slot].itemstacklimit;
									arraypassed[slot].itemstacksize = arraypassed[slot].itemstacklimit;
								} else {
									//need to chech here if together they are larger
									Object.Destroy(mouseitem.worldObject);
									mouseitem = null;
								}
								//do something
							} else {
								//this take the variable from this slot and makes it temporary so we can put it in to mouse cursor
								InventoryItem tempaa = arraypassed[slot];
								//what ever is in cursor will be put in the slot
								arraypassed[slot] = mouseitem;
								if (tempaa != null){ //IS THIS NECESARRY????????????
									mouseitem = tempaa;
								} else {
									mouseitem = null;
								}
							}
						} else {
							//this take the variable from this slot and makes it temporary so we can put it in to mouse cursor
							InventoryItem tempa = arraypassed[slot];
							//what ever is in cursor will be put in the slot
							
							string toolpid = tooltipid.Substring(0, 5);
							//Debug.Log(toolpid);
							
							if(toolpid == (mouseitem.itemtype+"_")) {
								//Debug.Log("if('"+toolpid+"'=='"+mouseitem.itemtype+"')");
								setnewItem = true;
							} else {
								if(toolpid == "Invs_" || toolpid == "HotB_") {
									//Debug.Log("if('"+toolpid+"'==('Invs_' || 'HotB_')");
									setnewItem = true;
								} else {
									//Debug.Log("set new item = false");
									setnewItem = false;
								}
							}
							
							if(setnewItem) {
								arraypassed[slot] = mouseitem;
								if (tempa != null){ //IS THIS NECESARRY????????????
									mouseitem = tempa;
								} else {
									mouseitem = null;
								}
							}
						}
					}
				}
				//this is where we do the logic for right mouse click when we have an item in hand
				if (Input.GetKeyUp(KeyCode.Mouse1)) {
					if (arraypassed[slot] != null){
						if (arraypassed[slot].itemtype == ItemTypes.Bag){
							if (arraypassed[slot].showBag){
								arraypassed[slot].showBag = false;
							} else {
								arraypassed[slot].showBag = true;
							}
						}
						if (arraypassed[slot].itemtype == ItemTypes.Equip){
							if (Input.GetKey(KeyCode.RightShift) || Input.GetKey(KeyCode.LeftShift)){
								rightClickEquip(arraypassed,slot,true);
							} else {
								rightClickEquip(arraypassed,slot,false);
							}
						}
						if (arraypassed[slot] != null && arraypassed[slot].usable == UseTypes.Consume){
							if (arraypassed[slot].itemtype != ItemTypes.Stack){
								//check timer
								Object.Destroy(arraypassed[slot].worldObject);
								arraypassed[slot] = null;
								//start timer
							} else {
								if (arraypassed[slot].itemstacksize == 1){
									Object.Destroy(arraypassed[slot].worldObject);
									arraypassed[slot] = null;
									//start timer
								} else {
									arraypassed[slot].itemstacksize = arraypassed[slot].itemstacksize - 1;
									//start timer
								}
							}
						}
						if (arraypassed[slot] != null && arraypassed[slot].usable == UseTypes.Use){
							if (arraypassed[slot].itemtype != ItemTypes.Equip){
								//do effect
								Debug.Log ("DO EFFECT");
								//start timer
							}
						}
					}
				}
			}
		}
		if (arraypassed[slot] != null){
			//if(mouseitem == null) {
			GUI.DrawTexture ( new Rect(xpos+1,ypos+1,iconSizeW, iconSizeH),arraypassed[slot].itemtex);
			//}
			
			GUIStyle stackstyle = new GUIStyle();
			
			stackstyle.fontStyle = FontStyle.Bold;
			stackstyle.normal.textColor = Color.white;
			stackstyle.fontSize = 12;
			stackstyle.alignment = TextAnchor.LowerRight;
			
			//I may be able to use a style that can do right to left alignment
			int stacksize = 0;
			stacksize = stacksize + arraypassed[slot].itemstacksize;
			
			if(arraypassed[slot].itemstacksize > 1) {
				GUI.Label( new Rect(xpos,ypos,iconSizeW, iconSizeH), stacksize.ToString(), stackstyle);
			}
		}
		if (mouseheld){
			mouseOver = GUI.tooltip; 
		}
		if (arraypassed[slot] != null){
			if (arraypassed[slot].itemtype == ItemTypes.Stack){
				if (arraypassed[slot].showStack){
					int aaa = 0;
					aaa = aaa + stackamount;
					//some kind of drag mouse increase decrease motion
					GUI.Box ( new Rect(900,700,100,50), "StackItemSelector");
					GUI.DrawTexture ( new Rect(900,700,100,50),emptyTex);
					GUI.Label( new Rect(940,700,100,100), aaa.ToString());
					if (GUI.Button( new Rect(900,710,20,20),"<")){
						if (stackamount > 1){
							stackamount --;
						}
					}
					if (GUI.Button( new Rect(980,710,20,20),">")){
						if (stackamount < arraypassed[slot].itemstacklimit){
							if (stackamount < arraypassed[slot].itemstacksize){
								stackamount ++;
							}
						}
					}
					if (GUI.Button( new Rect(900,730,50,20),"Okay")){
						arraypassed[slot].showStack = false;
						if (arraypassed[slot].itemstacksize == stackamount){
							arraypassed[slot].itemstacksize = stackamount;
							mouseitem = arraypassed[slot];
							arraypassed[slot] = null;
						} else {
							arraypassed[slot].itemstacksize = arraypassed[slot].itemstacksize - stackamount;
							newStack(slot,stackamount,arraypassed);
						} //when you click on another one you just take the mouse items stack size add it to the inventory and then make the mouse null
					}// you first check that its not too large otherwise you just add the item to it and then when you click again it would swap them
					if (GUI.Button( new Rect(950,730,50,20),"Cancel")){
						arraypassed[slot].showStack = false;
					}
				}
			}
		}
		itemToolTip(slot,xpos,ypos,arraypassed,tooltipid);
	}

	//This is only for armor
	public void equipedItemSlot(int slot, float xpos, float ypos, EquipmentType itemname){
		
		string tooltipid = "a";
		tooltipid = tooltipid + slot;
		
		/*if (itemname == "Ammo") {
			//inv = new InventoryItem[Slots.AmmoSlots.Length];
			inv = Slots.AmmoSlots;	
		}*/ /*if (itemname == "Accs") {
			//inv = new InventoryItem[Slots.Accesories.Length];
			inv = Slots.Accesories;		
		}*/

		if(itemname == EquipmentType.None) {
			return;
		} //Else... Set the inv value
		
		if (inv[slot] == null){
			if (ItemBox(xpos, ypos, "")) { //itemname)) {//GUI.Button( new Rect(xpos,ypos,iconSizeW, iconSizeH), itemname)){
				if (Input.GetKeyUp(KeyCode.Mouse0)) {
					if (mouseitem != null && mouseitem.itemtype == ItemTypes.Equip){
						if (mouseitem.equipmenttype == itemname){
							inv[slot] = mouseitem;
							mouseitem = null;
						}
					}
				}
			}
		} else {
			if (ItemBox(xpos, ypos, "", tooltipid)) {//GUI.Button( new Rect(xpos,ypos,iconSizeW, iconSizeH), new GUIContent("", tooltipid))) {
				if (Input.GetKeyUp(KeyCode.Mouse0)) {
					if (mouseitem != null){
						if (mouseitem.itemtype == ItemTypes.Equip){
							if (mouseitem.equipmenttype == itemname){
								InventoryItem tempb = inv[slot];
								inv[slot] = mouseitem;
								mouseitem = tempb;
							}
						}
					} else {
						mouseitem = inv[slot];
						inv[slot] = null;
					}
				}
				if (Input.GetKeyUp(KeyCode.Mouse1)) {
					if (inv[slot] != null && inv[slot].usable == UseTypes.Use){//check timer too
						//do effect
						Debug.Log ("DO EFFECT");
						//start timer
					}
				}
			}
			if (inv[slot] != null ){
				GUI.DrawTexture ( new Rect(xpos,ypos,iconSizeW, iconSizeH), inv[slot].itemtex);
			}
			if (mouseheld){
				mouseOver = GUI.tooltip; 
			}
			itemToolTip(slot,xpos,ypos,Slots.EquipedItem,tooltipid);
		}
	}
	
	
	
	public void rightClickEquip (InventoryItem[] item, int slot, bool shiftpressed){

		//This is wrong, I have to edit it...
		/*if (item[slot] != null && item[slot].equipmenttype == "Accs") {
			#pragma warning disable
			for(int x=0;x<LvlSys.AccsX*LvlSys.AccsY;x++) {
				if(Slots.Accesories[x] == null) {
					InventoryItem tempa = Slots.Accesories[x];
					Slots.Accesories[x] = item[slot];
					item[slot] = tempa;
					break;
				} else {
					return;
				}
			}
			#pragma warning restore
		}
		
		if (item[slot] != null && item[slot].equipmenttype == "Ammo") {
			#pragma warning disable
			for(int x=0;x<LvlSys.Ammo;x++) {
				if(Slots.Accesories[x] == null) {
					InventoryItem tempb = Slots.AmmoSlots[x];
					Slots.AmmoSlots[x] = item[slot];
					item[slot] = tempb;
					break;
				} else {
					return;
				}
			}
			#pragma warning restore
		}*/
		
	}

	public void bagItemSlot (int slot, float xpos, float ypos, InventoryItem[] bagarray, int toolnumber){
		
		string tooltipid = "";
		bool hasitem;
		
		if (toolnumber == 0){
			tooltipid = "b";
		}
		if (toolnumber == 1){
			tooltipid = "c";
		}
		if (toolnumber == 2){
			tooltipid = "d";
		}
		if (toolnumber == 3){
			tooltipid = "e";
		}
		if (toolnumber == 4){
			tooltipid = "f";
		}
		if (toolnumber == 5){
			tooltipid = "g";
		}
		if (toolnumber== 6){
			tooltipid = "h";
		}
		if (toolnumber == 7){
			tooltipid = "i";
		}
		
		if (toolnumber > 7) {
			tooltipid = "x";		
		}
		
		tooltipid = tooltipid + slot;
		
		if (bagarray[slot] != null && mouseitem == null){
			
			if (GUI.Button( new Rect(xpos,ypos,32,32), new GUIContent("", tooltipid))){
				if (Input.GetKeyUp(KeyCode.Mouse0)) {
					stackCheck(bagarray);
					mouseitem = bagarray[slot];
					bagarray[slot] = null;
				}
				if (Input.GetKeyUp(KeyCode.Mouse1)){
					if (bagarray[slot] != null){
						if (bagarray[slot].itemtype == ItemTypes.Stack){
							if (Input.GetKey(KeyCode.RightShift) || Input.GetKey(KeyCode.LeftShift)){
								//DO SOMETHING IF SHIFT LEFT CLICK HAPPENS
								stackamount = 1;
								stackCheck(bagarray);
								bagarray[slot].showStack = true;
							}
						}
						if (bagarray[slot].itemtype == ItemTypes.Equip){
							if (Input.GetKey(KeyCode.RightShift) || Input.GetKey(KeyCode.LeftShift)){
								rightClickEquip(bagarray,slot,true);
							} else {
								rightClickEquip(bagarray,slot,false);
							}
						}
						if (bagarray[slot]!= null){
							if (bagarray[slot] != null && bagarray[slot].usable == UseTypes.Consume){
								if (bagarray[slot].itemtype != ItemTypes.Stack){
									//check timer
									Object.Destroy(bagarray[slot].worldObject);
									bagarray[slot] = null;
									//start timer
								} else {
									if (bagarray[slot].itemstacksize == 1){
										Object.Destroy(bagarray[slot].worldObject);
										bagarray[slot] = null;
										//start timer
									} else {
										bagarray[slot].itemstacksize = bagarray[slot].itemstacksize - 1;
										//start timer
									}
								}
							}
							if (bagarray[slot] != null && bagarray[slot].usable == UseTypes.Use){
								if (bagarray[slot].itemtype != ItemTypes.Equip){
									//do effect
									Debug.Log ("DO EFFECT");
									//start timer
								}
							}
						}
					}
				}
			}
			
		} else {
			if (GUI.Button( new Rect(xpos,ypos,32,32), new GUIContent("", tooltipid))){
				if (Input.GetKeyUp(KeyCode.Mouse0)) {
					
					if (mouseitem != null){	
						if (mouseitem.itemtype != ItemTypes.Bag){
							
							
							if (bagarray[slot] != null && bagarray[slot].itemtype == ItemTypes.Stack && mouseitem.itemtype == ItemTypes.Stack && bagarray[slot].itemname == mouseitem.itemname ){//&& check if they are both the same type of stack else it
								
								if (bagarray[slot].itemstacksize < bagarray[slot].itemstacklimit){
									
									bagarray[slot].itemstacksize = bagarray[slot].itemstacksize + mouseitem.itemstacksize;
									if (bagarray[slot].itemstacksize > bagarray[slot].itemstacklimit){
										mouseitem.itemstacksize = bagarray[slot].itemstacksize - bagarray[slot].itemstacklimit;
										bagarray[slot].itemstacksize = bagarray[slot].itemstacklimit;
									} else {
										//need to chech here if together they are larger
										Object.Destroy(mouseitem.worldObject);
										mouseitem = null;
									}
									//do something
								} else {
									InventoryItem tempaa= bagarray[slot];
									//what ever is in cursor will be put in the slot
									bagarray[slot] = mouseitem;
									if (tempaa != null){
										mouseitem = tempaa;
									} else {
										mouseitem = null;
									}
								}
							} else {
								InventoryItem tempa= bagarray[slot];
								//what ever is in cursor will be put in the slot
								bagarray[slot] = mouseitem;
								if (tempa != null){
									mouseitem = tempa;
								} else {
									mouseitem = null;
								}
							}
						} else {
							hasitem = bagCheck();
							if (!hasitem){
								InventoryItem tempb= bagarray[slot];
								//what ever is in cursor will be put in the slot
								bagarray[slot] = mouseitem;
								if (tempb != null){
									mouseitem = tempb;
								} else {
									mouseitem = null;
								}
							}
						}
					}	
				}
				if (Input.GetKeyUp(KeyCode.Mouse1)) {
					if (bagarray[slot] != null && bagarray[slot].itemtype == ItemTypes.Equip){
						if (Input.GetKey(KeyCode.RightShift) || Input.GetKey(KeyCode.LeftShift)){
							rightClickEquip(bagarray,slot,true);
						} else {
							rightClickEquip(bagarray,slot,false);
						}
					}
					if (bagarray[slot] != null && bagarray[slot].usable == UseTypes.Consume){
						if (bagarray[slot].itemtype != ItemTypes.Stack){
							//check timer
							Object.Destroy(bagarray[slot].worldObject);
							bagarray[slot] = null;
							//start timer
						} else {
							if (bagarray[slot].itemstacksize == 1){
								Object.Destroy(bagarray[slot].worldObject);
								bagarray[slot] = null;
								//start timer
							} else {
								bagarray[slot].itemstacksize = bagarray[slot].itemstacksize - 1;
								//start timer
							}
						}
					}
					if (bagarray[slot] != null && bagarray[slot].usable == UseTypes.Use){
						if (bagarray[slot].itemtype != ItemTypes.Equip){
							//do effect
							Debug.Log ("DO EFFECT");
							//start timer
						}
					}
				}
			}
		}
		if (bagarray[slot] !=null){
			GUI.DrawTexture ( new Rect(xpos,ypos,32,32),bagarray[slot].itemtex);
			
			//I may be able to use a style that can do right to left alignment
			if (bagarray[slot].itemtype == ItemTypes.Stack){
				int stacksizeb = 0;
				stacksizeb = stacksizeb + bagarray[slot].itemstacksize;
				int stackspace = 0;
				if (stacksizeb == 3){
					stackspace = 12;
				}
				if (stacksizeb == 2){
					stackspace = 19;
				}
				if (stacksizeb == 1){
					stackspace = 26;
				}
				GUI.Label( new Rect(xpos+stackspace,ypos+19,iconSizeW, iconSizeH), stacksizeb.ToString());
			}
			
		}
		
		if (mouseheld){
			mouseOver = GUI.tooltip; 
		}
		if (bagarray[slot] != null){
			if (bagarray[slot].itemtype == ItemTypes.Stack){
				if (bagarray[slot].showStack){
					int aaa = 0;
					aaa = aaa + stackamount;
					//some kind of drag mouse increase decrease motion
					GUI.DrawTexture ( new Rect(900,700,100,50),emptyTex);
					GUI.Label( new Rect(940,700,100,100), aaa.ToString());
					if (GUI.Button( new Rect(900,710,20,20),"<")){
						if (stackamount > 1){
							stackamount --;
						}
					}
					if (GUI.Button( new Rect(980,710,20,20),">")){
						if (stackamount < bagarray[slot].itemstacklimit){
							if (stackamount < bagarray[slot].itemstacksize){
								stackamount ++;
							}
						}
					}
					if (GUI.Button( new Rect(900,730,50,20),"Okay")){
						bagarray[slot].showStack = false;
						if (bagarray[slot].itemstacksize == stackamount){
							bagarray[slot].itemstacksize = stackamount;
							mouseitem = bagarray[slot];
							bagarray[slot] = null;
						} else {
							bagarray[slot].itemstacksize = bagarray[slot].itemstacksize - stackamount;
							newStack(slot,stackamount,bagarray);
						} //when you click on another one you just take the mouse items stack size add it to the inventory and then make the mouse null
					}// you first check that its not too large otherwise you just add the item to it and then when you click again it would swap them
					if (GUI.Button( new Rect(950,730,50,20),"Cancel")){
						bagarray[slot].showStack = false;
					}
				}
			}
		}
		itemToolTip(slot,xpos,ypos,bagarray,tooltipid);
	}
	
	
	//This is part of the process of makeing a bag
	public void makeBag (float xpos, float ypos, int bagsize, Texture bagtex, InventoryItem[] arraypassed, string bagsname, InventoryItem[] passedarray, int slot){
		int slotnumber = -1;
		GUI.Box ( new Rect(xpos,ypos,163,bagsize*37 + 35), "");//this scales the texture too
		GUI.DrawTexture ( new Rect(xpos,ypos,163,bagsize*37 + 35), bagtex);
		GUI.Label( new Rect(xpos,ypos,163,30),bagsname);
		for(int i = 0; i < bagsize; i++){
			for(int j = 0; j < 4; j++){//the size of the horizontal bag size
				slotnumber ++;
				float xx = 32*j + 5*(j+1) + 5 + xpos;
				float yy = 32*i + 5*(i+1) + 30 + ypos;
				bagItemSlot(slotnumber,xx,yy,arraypassed,slot);
			}
		}
		if (GUI.Button( new Rect(xpos + 138,ypos + 5,20,20),"X")){
			if (passedarray[slot].showBag){
				passedarray[slot].showBag = false;
			} else {
				passedarray[slot].showBag = true;
			}
		}
	}
	
	
	//This is just a function that checks if a bag is in an inventory slot and its showbag is set to true, it will call a function to further open the bags and position them accordingly
	public void callBag (InventoryItem[] arraypassed, int slot, float xpos, float ypos){
		if (arraypassed[slot] != null && arraypassed[slot].itemtype == ItemTypes.Bag && arraypassed[slot].showBag == true){
			makeBag(xpos,ypos,arraypassed[slot].bagsize,bagtexture,arraypassed[slot].BagItem,arraypassed[slot].itemname,arraypassed,slot);
		}
	}
	
	//This checks if there is items in the bag that is in the mouse hand so that we cant put bags with items in to other bags
	public bool bagCheck (){
		for(int i = 0; i < mouseitem.BagItem.Length; i++){
			if (mouseitem.BagItem[i] != null){
				return true;
			}
		}
		return false;
	}
	
	public void stackCheck (InventoryItem[] inventory){
		for(int i = 0; i < inventory.Length; i++){
			if (inventory[i] != null){
				if (inventory[i].itemtype == ItemTypes.Stack){
					if (inventory[i].showStack){
						inventory[i].showStack = false;
					}
				}
				if (inventory[i].itemtype == ItemTypes.Bag){
					for (int k = 0; k < 20; k++){
						if (inventory[i].BagItem[k] != null){
							if (inventory[i].BagItem[k].itemtype == ItemTypes.Stack){
								inventory[i].BagItem[k].showStack = false;
							}
						}
					}
				}
			}
		}
	}
	//This toggles all bags in inventory when it is called
	public void openAllBags (InventoryItem[] inventory){
		int bagcount = 0;
		int bagsopen = 0;
		for( int i = 0; i < inventory.Length; i ++ ){
			if (inventory[i] != null){
				if (inventory[i].itemtype == ItemTypes.Bag){
					bagcount ++;
					if (inventory[i].showBag == true){
						bagsopen ++;
					}
					inventory[i].showBag = true;
				}
			}
		}
		
		if (bagcount != bagsopen || bagsopen == 0){
			for( int k = 0; k < inventory.Length; k ++ ){
				if (inventory[k] != null){
					if (inventory[k].itemtype == ItemTypes.Bag){
						inventory[k].showBag = true;
					}
				}
			}
		} else {
			for( int j = 0; j < inventory.Length; j ++ ){
				if (inventory[j] != null){
					if (inventory[j].itemtype == ItemTypes.Bag){
						inventory[j].showBag = false;
					}
				}
			}
		}
	}
	
	//This is the item tooltip function for dispaying a tooltip when hovering over items
	//CAN PERHAPS PUT THE FIRST IF STATEMENT OUTSIDE OF THIS SO THIS DOESNT RUN
	public void itemToolTip(int slot, float xpos, float ypos, InventoryItem[] arraypassed, string tooltipid)  {
		/*if(mouseOver==tooltipid) {
			if (Input.GetKey(KeyCode.RightShift) == false && Input.GetKey(KeyCode.LeftShift) == false){
				tooltipitem1 = arraypassed[slot];
				if (xpos-100 > 0){
					tooltipx1 = xpos-100;
				} else {
					tooltipx1 = xpos + iconSizeW;
				}
				if (ypos-100 > 0){
					tooltipy1 = ypos-100;
				} else {
					tooltipy1 = ypos + iconSizeH;
				}
			} else {*/
				//I have to edit this...
				/*if (arraypassed[slot] != null){
					if (arraypassed[slot].itemtype == ItemTypes.Equip){
						if(arraypassed[slot].equipmenttype == EquipmentType.Hat){
							if (Slots.EquipedItem[0] != null){
								tooltipitem1 = arraypassed[slot];
								tooltipitem2 = Slots.EquipedItem[0];
								if (xpos-200 > 0){
									tooltipx1 = xpos-100;
									tooltipx2 = xpos-200;
								} else {
									tooltipx1 = xpos + iconSizeW;
									tooltipx2 = xpos + iconSizeW + 100;
								}
								if (ypos-100 > 0){
									tooltipy1 = ypos-100;
									tooltipy2 = ypos-100;
								} else {
									tooltipy1 = ypos + iconSizeH;
									tooltipy2 = ypos + iconSizeH;
								}
							}
							if (Slots.EquipedItem[0] == null){
								tooltipitem1 = arraypassed[slot];
								if (xpos-100 > 0){
									tooltipx1 = xpos-100;
								} else {
									tooltipx1 = xpos + iconSizeW;
								}
								if (ypos-100 > 0){
									tooltipy1 = ypos-100;
								} else {
									tooltipy1 = ypos + iconSizeH;
								}
							}
						}
						if(arraypassed[slot].equipmenttype == "Neck"){
							if (Slots.EquipedItem[1] != null){
								tooltipitem1 = arraypassed[slot];
								tooltipitem2 = Slots.EquipedItem[1];
								if (xpos-200 > 0){
									tooltipx1 = xpos-100;
									tooltipx2 = xpos-200;
								} else {
									tooltipx1 = xpos + iconSizeW;
									tooltipx2 = xpos + iconSizeW + 100;
								}
								if (ypos-100 > 0){
									tooltipy1 = ypos-100;
									tooltipy2 = ypos-100;
								} else {
									tooltipy1 = ypos + iconSizeH;
									tooltipy2 = ypos + iconSizeH;
								}
							}
							if (Slots.EquipedItem[1] == null){
								tooltipitem1 = arraypassed[slot];
								if (xpos-100 > 0){
									tooltipx1 = xpos-100;
								} else {
									tooltipx1 = xpos + iconSizeW;
								}
								if (ypos-100 > 0){
									tooltipy1 = ypos-100;
								} else {
									tooltipy1 = ypos + iconSizeH;
								}
							}
						}
						if(arraypassed[slot].equipmenttype == "Shoulders"){
							if (Slots.EquipedItem[2] != null){
								tooltipitem1 = arraypassed[slot];
								tooltipitem2 = Slots.EquipedItem[2];
								if (xpos-200 > 0){
									tooltipx1 = xpos-100;
									tooltipx2 = xpos-200;
								} else {
									tooltipx1 = xpos + iconSizeW;
									tooltipx2 = xpos + iconSizeW + 100;
								}
								if (ypos-100 > 0){
									tooltipy1 = ypos-100;
									tooltipy2 = ypos-100;
								} else {
									tooltipy1 = ypos + iconSizeH;
									tooltipy2 = ypos + iconSizeH;
								}
							}
							if (Slots.EquipedItem[2] == null){
								tooltipitem1 = arraypassed[slot];
								if (xpos-100 > 0){
									tooltipx1 = xpos-100;
								} else {
									tooltipx1 = xpos + iconSizeW;
								}
								if (ypos-100 > 0){
									tooltipy1 = ypos-100;
								} else {
									tooltipy1 = ypos + iconSizeH;
								}
							}
						}
						if(arraypassed[slot].equipmenttype == "Back"){
							if (Slots.EquipedItem[3] != null){
								tooltipitem1 = arraypassed[slot];
								tooltipitem2 = Slots.EquipedItem[3];
								if (xpos-200 > 0){
									tooltipx1 = xpos-100;
									tooltipx2 = xpos-200;
								} else {
									tooltipx1 = xpos + iconSizeW;
									tooltipx2 = xpos + iconSizeW + 100;
								}
								if (ypos-100 > 0){
									tooltipy1 = ypos-100;
									tooltipy2 = ypos-100;
								} else {
									tooltipy1 = ypos + iconSizeH;
									tooltipy2 = ypos + iconSizeH;
								}
							}
							if (Slots.EquipedItem[3] == null){
								tooltipitem1 = arraypassed[slot];
								if (xpos-100 > 0){
									tooltipx1 = xpos-100;
								} else {
									tooltipx1 = xpos + iconSizeW;
								}
								if (ypos-100 > 0){
									tooltipy1 = ypos-100;
								} else {
									tooltipy1 = ypos + iconSizeH;
								}
							}
						}
						if(arraypassed[slot].equipmenttype == "Chest"){
							if (Slots.EquipedItem[4] != null){
								tooltipitem1 = arraypassed[slot];
								tooltipitem2 = Slots.EquipedItem[4];
								if (xpos-200 > 0){
									tooltipx1 = xpos-100;
									tooltipx2 = xpos-200;
								} else {
									tooltipx1 = xpos + iconSizeW;
									tooltipx2 = xpos + iconSizeW + 100;
								}
								if (ypos-100 > 0){
									tooltipy1 = ypos-100;
									tooltipy2 = ypos-100;
								} else {
									tooltipy1 = ypos + iconSizeH;
									tooltipy2 = ypos + iconSizeH;
								}
							}
							if (Slots.EquipedItem[4] == null){
								tooltipitem1 = arraypassed[slot];
								if (xpos-100 > 0){
									tooltipx1 = xpos-100;
								} else {
									tooltipx1 = xpos + iconSizeW;
								}
								if (ypos-100 > 0){
									tooltipy1 = ypos-100;
								} else {
									tooltipy1 = ypos + iconSizeH;
								}
							}
						}
						if(arraypassed[slot].equipmenttype == "Waist"){
							if (Slots.EquipedItem[5] != null){
								tooltipitem1 = arraypassed[slot];
								tooltipitem2 = Slots.EquipedItem[5];
								if (xpos-200 > 0){
									tooltipx1 = xpos-100;
									tooltipx2 = xpos-200;
								} else {
									tooltipx1 = xpos + iconSizeW;
									tooltipx2 = xpos + iconSizeW + 100;
								}
								if (ypos-100 > 0){
									tooltipy1 = ypos-100;
									tooltipy2 = ypos-100;
								} else {
									tooltipy1 = ypos + iconSizeH;
									tooltipy2 = ypos + iconSizeH;
								}
							}
							if (Slots.EquipedItem[5] == null){
								tooltipitem1 = arraypassed[slot];
								if (xpos-100 > 0){
									tooltipx1 = xpos-100;
								} else {
									tooltipx1 = xpos + iconSizeW;
								}
								if (ypos-100 > 0){
									tooltipy1 = ypos-100;
								} else {
									tooltipy1 = ypos + iconSizeH;
								}
							}
						}
						if(arraypassed[slot].equipmenttype == "Legs"){
							if (Slots.EquipedItem[6] != null){
								tooltipitem1 = arraypassed[slot];
								tooltipitem2 = Slots.EquipedItem[6];
								if (xpos-200 > 0){
									tooltipx1 = xpos-100;
									tooltipx2 = xpos-200;
								} else {
									tooltipx1 = xpos + iconSizeW;
									tooltipx2 = xpos + iconSizeW + 100;
								}
								if (ypos-100 > 0){
									tooltipy1 = ypos-100;
									tooltipy2 = ypos-100;
								} else {
									tooltipy1 = ypos + iconSizeH;
									tooltipy2 = ypos + iconSizeH;
								}
							}
							if (Slots.EquipedItem[6] == null){
								tooltipitem1 = arraypassed[slot];
								if (xpos-100 > 0){
									tooltipx1 = xpos-100;
								} else {
									tooltipx1 = xpos + iconSizeW;
								}
								if (ypos-100 > 0){
									tooltipy1 = ypos-100;
								} else {
									tooltipy1 = ypos + iconSizeH;
								}
							}
						}
						if(arraypassed[slot].equipmenttype == "Feet"){
							if (Slots.EquipedItem[7] != null){
								tooltipitem1 = arraypassed[slot];
								tooltipitem2 = Slots.EquipedItem[7];
								if (xpos-200 > 0){
									tooltipx1 = xpos-100;
									tooltipx2 = xpos-200;
								} else {
									tooltipx1 = xpos + iconSizeW;
									tooltipx2 = xpos + iconSizeW + 100;
								}
								if (ypos-100 > 0){
									tooltipy1 = ypos-100;
									tooltipy2 = ypos-100;
								} else {
									tooltipy1 = ypos + iconSizeH;
									tooltipy2 = ypos + iconSizeH;
								}
							}
							if (Slots.EquipedItem[7] == null){
								tooltipitem1 = arraypassed[slot];
								if (xpos-100 > 0){
									tooltipx1 = xpos-100;
								} else {
									tooltipx1 = xpos + iconSizeW;
								}
								if (ypos-100 > 0){
									tooltipy1 = ypos-100;
								} else {
									tooltipy1 = ypos + iconSizeH;
								}
							}
						}
						if(arraypassed[slot].equipmenttype == "Arms"){
							if (Slots.EquipedItem[8] != null){
								tooltipitem1 = arraypassed[slot];
								tooltipitem2 = Slots.EquipedItem[8];
								if (xpos-200 > 0){
									tooltipx1 = xpos-100;
									tooltipx2 = xpos-200;
								} else {
									tooltipx1 = xpos + iconSizeW;
									tooltipx2 = xpos + iconSizeW + 100;
								}
								if (ypos-100 > 0){
									tooltipy1 = ypos-100;
									tooltipy2 = ypos-100;
								} else {
									tooltipy1 = ypos + iconSizeH;
									tooltipy2 = ypos + iconSizeH;
								}
							}
							if (Slots.EquipedItem[8] == null){
								tooltipitem1 = arraypassed[slot];
								if (xpos-100 > 0){
									tooltipx1 = xpos-100;
								} else {
									tooltipx1 = xpos + iconSizeW;
								}
								if (ypos-100 > 0){
									tooltipy1 = ypos-100;
								} else {
									tooltipy1 = ypos + iconSizeH;
								}
							}
						}
						if(arraypassed[slot].equipmenttype == "Hands"){
							if (Slots.EquipedItem[9] != null){
								tooltipitem1 = arraypassed[slot];
								tooltipitem2 = Slots.EquipedItem[9];
								if (xpos-200 > 0){
									tooltipx1 = xpos-100;
									tooltipx2 = xpos-200;
								} else {
									tooltipx1 = xpos + iconSizeW;
									tooltipx2 = xpos + iconSizeW + 100;
								}
								if (ypos-100 > 0){
									tooltipy1 = ypos-100;
									tooltipy2 = ypos-100;
								} else {
									tooltipy1 = ypos + iconSizeH;
									tooltipy2 = ypos + iconSizeH;
								}
							}
							if (Slots.EquipedItem[9] == null){
								tooltipitem1 = arraypassed[slot];
								if (xpos-100 > 0){
									tooltipx1 = xpos-100;
								} else {
									tooltipx1 = xpos + iconSizeW;
								}
								if (ypos-100 > 0){
									tooltipy1 = ypos-100;
								} else {
									tooltipy1 = ypos + iconSizeH;
								}
							}
						}
						if(arraypassed[slot].equipmenttype == "Wrist"){
							if (Slots.EquipedItem[10] != null){
								if (Slots.EquipedItem[11] != null){
									tooltipitem1 = arraypassed[slot];
									tooltipitem2 = Slots.EquipedItem[10];
									tooltipitem3 = Slots.EquipedItem[11];
									if (xpos-300 > 0){
										tooltipx1 = xpos-100;
										tooltipx2 = xpos-200;
										tooltipx3 = xpos-300;
									} else {
										tooltipx1 = xpos + iconSizeW;
										tooltipx2 = xpos + iconSizeW + 100;
										tooltipx3 = xpos + iconSizeW + 200;
									}
									if (ypos-100 > 0){
										tooltipy1 = ypos-100;
										tooltipy2 = ypos-100;
										tooltipy3 = ypos-100;
									} else {
										tooltipy1 = ypos + iconSizeH;
										tooltipy2 = ypos + iconSizeH;
										tooltipy3 = ypos + iconSizeH;
									}
								} else {
									tooltipitem1 = arraypassed[slot];
									tooltipitem2 = Slots.EquipedItem[10];
									if (xpos-200 > 0){
										tooltipx1 = xpos-100;
										tooltipx2 = xpos-200;
									} else {
										tooltipx1 = xpos + iconSizeW;
										tooltipx2 = xpos + iconSizeW + 100;
									}
									if (ypos-100 > 0){
										tooltipy1 = ypos-100;
										tooltipy2 = ypos-100;
									} else {
										tooltipy1 = ypos + iconSizeH;
										tooltipy2 = ypos + iconSizeH;
									}
								}
							} else {
								if (Slots.EquipedItem[11] != null){
									tooltipitem1 = arraypassed[slot];
									tooltipitem2 = Slots.EquipedItem[11];
									if (xpos-200 > 0){
										tooltipx1 = xpos-100;
										tooltipx2 = xpos-200;
									} else {
										tooltipx1 = xpos + iconSizeW;
										tooltipx2 = xpos + iconSizeW + 100;
									}
									if (ypos-100 > 0){
										tooltipy1 = ypos-100;
										tooltipy2 = ypos-100;
									} else {
										tooltipy1 = ypos + iconSizeH;
										tooltipy2 = ypos + iconSizeH;
									}
								}
							}
							if (Slots.EquipedItem[10] == null && Slots.EquipedItem[11] == null){
								tooltipitem1 = arraypassed[slot];
								if (xpos-100 > 0){
									tooltipx1 = xpos-100;
								} else {
									tooltipx1 = xpos + iconSizeW;
								}
								if (ypos-100 > 0){
									tooltipy1 = ypos-100;
								} else {
									tooltipy1 = ypos + iconSizeH;
								}
							}
						}
						if(arraypassed[slot].equipmenttype == "Finger"){
							if (Slots.EquipedItem[12] != null){
								if (Slots.EquipedItem[13] != null){
									tooltipitem1 = arraypassed[slot];
									tooltipitem2 = Slots.EquipedItem[12];
									tooltipitem3 = Slots.EquipedItem[13];
									if (xpos-300 > 0){
										tooltipx1 = xpos-100;
										tooltipx2 = xpos-200;
										tooltipx3 = xpos-300;
									} else {
										tooltipx1 = xpos + iconSizeW;
										tooltipx2 = xpos + iconSizeW + 100;
										tooltipx3 = xpos + iconSizeW + 200;
									}
									if (ypos-100 > 0){
										tooltipy1 = ypos-100;
										tooltipy2 = ypos-100;
										tooltipy3 = ypos-100;
									} else {
										tooltipy1 = ypos + iconSizeH;
										tooltipy2 = ypos + iconSizeH;
										tooltipy3 = ypos + iconSizeH;
									}
								} else {
									tooltipitem1 = arraypassed[slot];
									tooltipitem2 = Slots.EquipedItem[12];
									if (xpos-200 > 0){
										tooltipx1 = xpos-100;
										tooltipx2 = xpos-200;
									} else {
										tooltipx1 = xpos + iconSizeW;
										tooltipx2 = xpos + iconSizeW + 100;
									}
									if (ypos-100 > 0){
										tooltipy1 = ypos-100;
										tooltipy2 = ypos-100;
									} else {
										tooltipy1 = ypos + iconSizeH;
										tooltipy2 = ypos + iconSizeH;
									}
								}
							} else {
								if (Slots.EquipedItem[13] != null){
									tooltipitem1 = arraypassed[slot];
									tooltipitem2 = Slots.EquipedItem[13];
									if (xpos-200 > 0){
										tooltipx1 = xpos-100;
										tooltipx2 = xpos-200;
									} else {
										tooltipx1 = xpos + iconSizeW;
										tooltipx2 = xpos + iconSizeW + 100;
									}
									if (ypos-100 > 0){
										tooltipy1 = ypos-100;
										tooltipy2 = ypos-100;
									} else {
										tooltipy1 = ypos + iconSizeH;
										tooltipy2 = ypos + iconSizeH;
									}
								}
							}
							if (Slots.EquipedItem[12] == null && Slots.EquipedItem[13] == null){
								tooltipitem1 = arraypassed[slot];
								if (xpos-100 > 0){
									tooltipx1 = xpos-100;
								} else {
									tooltipx1 = xpos + iconSizeW;
								}
								if (ypos-100 > 0){
									tooltipy1 = ypos-100;
								} else {
									tooltipy1 = ypos + iconSizeH;
								}
							}
						}
						if(arraypassed[slot].equipmenttype == "Ear"){
							if (Slots.EquipedItem[14] != null){
								if (Slots.EquipedItem[15] != null){
									tooltipitem1 = arraypassed[slot];
									tooltipitem2 = Slots.EquipedItem[14];
									tooltipitem3 = Slots.EquipedItem[15];
									if (xpos-300 > 0){
										tooltipx1 = xpos-100;
										tooltipx2 = xpos-200;
										tooltipx3 = xpos-300;
									} else {
										tooltipx1 = xpos + iconSizeW;
										tooltipx2 = xpos + iconSizeW + 100;
										tooltipx3 = xpos + iconSizeW + 200;
									}
									if (ypos-100 > 0){
										tooltipy1 = ypos-100;
										tooltipy2 = ypos-100;
										tooltipy3 = ypos-100;
									} else {
										tooltipy1 = ypos + iconSizeH;
										tooltipy2 = ypos + iconSizeH;
										tooltipy3 = ypos + iconSizeH;
									}
								} else {
									tooltipitem1 = arraypassed[slot];
									tooltipitem2 = Slots.EquipedItem[14];
									if (xpos-200 > 0){
										tooltipx1 = xpos-100;
										tooltipx2 = xpos-200;
									} else {
										tooltipx1 = xpos + iconSizeW;
										tooltipx2 = xpos + iconSizeW + 100;
									}
									if (ypos-100 > 0){
										tooltipy1 = ypos-100;
										tooltipy2 = ypos-100;
									} else {
										tooltipy1 = ypos + iconSizeH;
										tooltipy2 = ypos + iconSizeH;
									}
								}
							} else {
								if (Slots.EquipedItem[15] != null){
									tooltipitem1 = arraypassed[slot];
									tooltipitem2 = Slots.EquipedItem[15];
									if (xpos-200 > 0){
										tooltipx1 = xpos-100;
										tooltipx2 = xpos-200;
									} else {
										tooltipx1 = xpos + iconSizeW;
										tooltipx2 = xpos + iconSizeW + 100;
									}
									if (ypos-100 > 0){
										tooltipy1 = ypos-100;
										tooltipy2 = ypos-100;
									} else {
										tooltipy1 = ypos + iconSizeH;
										tooltipy2 = ypos + iconSizeH;
									}
								}
							}
							if (Slots.EquipedItem[14] == null && Slots.EquipedItem[15] == null){
								tooltipitem1 = arraypassed[slot];
								if (xpos-100 > 0){
									tooltipx1 = xpos-100;
								} else {
									tooltipx1 = xpos + iconSizeW;
								}
								if (ypos-100 > 0){
									tooltipy1 = ypos-100;
								} else {
									tooltipy1 = ypos + iconSizeH;
								}
							}
						}
						if(arraypassed[slot].equipmenttype == "Primary"){
							if (Slots.EquipedItem[16] != null){
								tooltipitem1 = arraypassed[slot];
								tooltipitem2 = Slots.EquipedItem[16];
								if (xpos-200 > 0){
									tooltipx1 = xpos-100;
									tooltipx2 = xpos-200;
								} else {
									tooltipx1 = xpos + iconSizeW;
									tooltipx2 = xpos + iconSizeW + 100;
								}
								if (ypos-100 > 0){
									tooltipy1 = ypos-100;
									tooltipy2 = ypos-100;
								} else {
									tooltipy1 = ypos + iconSizeH;
									tooltipy2 = ypos + iconSizeH;
								}
							}
							if (Slots.EquipedItem[16] == null){
								tooltipitem1 = arraypassed[slot];
								if (xpos-100 > 0){
									tooltipx1 = xpos-100;
								} else {
									tooltipx1 = xpos + iconSizeW;
								}
								if (ypos-100 > 0){
									tooltipy1 = ypos-100;
								} else {
									tooltipy1 = ypos + iconSizeH;
								}
							}
						}
						if(arraypassed[slot].equipmenttype == "Secondary"){
							if (Slots.EquipedItem[17] != null){
								tooltipitem1 = arraypassed[slot];
								tooltipitem2 = Slots.EquipedItem[17];
								if (xpos-200 > 0){
									tooltipx1 = xpos-100;
									tooltipx2 = xpos-200;
								} else {
									tooltipx1 = xpos + iconSizeW;
									tooltipx2 = xpos + iconSizeW + 100;
								}
								if (ypos-100 > 0){
									tooltipy1 = ypos-100;
									tooltipy2 = ypos-100;
								} else {
									tooltipy1 = ypos + iconSizeH;
									tooltipy2 = ypos + iconSizeH;
								}
							}
							if (Slots.EquipedItem[17] == null){
								tooltipitem1 = arraypassed[slot];
								if (xpos-100 > 0){
									tooltipx1 = xpos-100;
								} else {
									tooltipx1 = xpos + iconSizeW;
								}
								if (ypos-100 > 0){
									tooltipy1 = ypos-100;
								} else {
									tooltipy1 = ypos + iconSizeH;
								}
							}
						}
						if(arraypassed[slot].equipmenttype == "Range"){
							if (Slots.EquipedItem[18] != null){
								tooltipitem1 = arraypassed[slot];
								tooltipitem2 = Slots.EquipedItem[18];
								if (xpos-200 > 0){
									tooltipx1 = xpos-100;
									tooltipx2 = xpos-200;
								} else {
									tooltipx1 = xpos + iconSizeW;
									tooltipx2 = xpos + iconSizeW + 100;
								}
								if (ypos-100 > 0){
									tooltipy1 = ypos-100;
									tooltipy2 = ypos-100;
								} else {
									tooltipy1 = ypos + iconSizeH;
									tooltipy2 = ypos + iconSizeH;
								}
							}
							if (Slots.EquipedItem[18] == null){
								tooltipitem1 = arraypassed[slot];
								if (xpos-100 > 0){
									tooltipx1 = xpos-100;
								} else {
									tooltipx1 = xpos + iconSizeW;
								}
								if (ypos-100 > 0){
									tooltipy1 = ypos-100;
								} else {
									tooltipy1 = ypos + iconSizeH;
								}
							}
						}
						if(arraypassed[slot].equipmenttype == "Ammo"){
							if (Slots.EquipedItem[19] != null){
								tooltipitem1 = arraypassed[slot];
								tooltipitem2 = Slots.EquipedItem[19];
								if (xpos-200 > 0){
									tooltipx1 = xpos-100;
									tooltipx2 = xpos-200;
								} else {
									tooltipx1 = xpos + iconSizeW;
									tooltipx2 = xpos + iconSizeW + 100;
								}
								if (ypos-100 > 0){
									tooltipy1 = ypos-100;
									tooltipy2 = ypos-100;
								} else {
									tooltipy1 = ypos + iconSizeH;
									tooltipy2 = ypos + iconSizeH;
								}
							}
							if (Slots.EquipedItem[19] == null){
								tooltipitem1 = arraypassed[slot];
								if (xpos-100 > 0){
									tooltipx1 = xpos-100;
								} else {
									tooltipx1 = xpos + iconSizeW;
								}
								if (ypos-100 > 0){
									tooltipy1 = ypos-100;
								} else {
									tooltipy1 = ypos + iconSizeH;
								}
							}
						}
					} else {
						tooltipitem1 = arraypassed[slot];
						if (xpos-100 > 0){
							tooltipx1 = xpos-100;
						} else {
							tooltipx1 = xpos + iconSizeW;
						}
						if (ypos-100 > 0){
							tooltipy1 = ypos-100;
						} else {
							tooltipy1 = ypos + iconSizeH;
						}
					}
				}
			}
		}*/
	}
	
	
	public float calculateweight (InventoryItem[] inventory){
		float weightamount = 0.0f;
		for( int i = 0; i < inventory.Length; i ++ ){
			if (inventory[i] != null){
				if (inventory[i].itemtype == ItemTypes.Bag){
					weightamount = weightamount + inventory[i].itemweight;
					for( int k = 0; k < 28; k ++ ){
						if (inventory[i].BagItem[k] != null){
							if (inventory[i].BagItem[k].itemtype == ItemTypes.Stack){
								weightamount = weightamount + (inventory[i].BagItem[k].itemweight * inventory[i].BagItem[k].itemstacksize);
							} else {
								weightamount = weightamount + inventory[i].BagItem[k].itemweight;
							}
						}
					}
				} else {
					if (inventory[i].itemtype == ItemTypes.Stack){
						weightamount = weightamount + (inventory[i].itemweight * inventory[i].itemstacksize);
					} else {
						weightamount = weightamount + inventory[i].itemweight;
					}
				}
			}
		}
		for( int j = 0; j < Slots.EquipedItem.Length; j ++ ){
			if (Slots.EquipedItem[j] != null){
				weightamount = weightamount + Slots.EquipedItem[j].itemweight;
			}
		}
		if (mouseitem != null){
			if (mouseitem.itemtype == ItemTypes.Stack){
				weightamount = weightamount + (mouseitem.itemweight * mouseitem.itemstacksize);
			} else {
				weightamount = weightamount + mouseitem.itemweight;
			}
		}
		
		float theweight = 0;
		theweight = theweight + weightamount;
		return theweight;
	}
	
	private GameObject blah;
	
	private void DropSelectedItem(int slot) {
		
		blah = new GameObject();
		
		InventoryItem selectedItem = Slots.HotBarSlots[slot];
		blah.name = "Item_"+selectedItem.itemname;
		
		if (mouseitem.dropObject == null) {
			blah = selectedItem.dropObject;
		} else {
			blah = selectedItem.dropObject;
		}
		
		if (Player.ThirdView) {
			playerObject = GameObject.Find ("Third Person Controller");
		} else {
			playerObject = GameObject.Find ("Player");	
		}
		
		player = playerObject.transform;
		
		if (mouseitem.dropObject == null) {
			GameObject.Instantiate(blah, player.transform.position, Quaternion.identity);
		} else {
			GameObject.Instantiate(blah, player.transform.position, Quaternion.identity);
		}
	}
	
	private void DropItem() { //InvItemToScene
		
		blah = new GameObject();
		
		blah.name = "Item_"+mouseitem.id;
		
		if (mouseitem.dropObject == null) {
			blah = mouseitem.worldObject;
		} else {
			blah = mouseitem.dropObject;
		}
		
		if (Player.ThirdView) {
			playerObject = GameObject.Find ("Third Person Controller");
		} else {
			playerObject = GameObject.Find ("Player");	
		}
		
		player = playerObject.transform;
		
		if (mouseitem.dropObject == null) {
			GameObject.Instantiate(blah, player.transform.position, Quaternion.identity);
		} else {
			GameObject.Instantiate(blah, player.transform.position, Quaternion.identity);
		}
	}
	
	private bool IsInventory(Vector2 Pos) {
		
		if (Slots.AccesoriesRect.Contains(Pos)) {
			return true;
		} else if(Slots.AmmoSlotsRect.Contains(Pos)) {
			return true;
		} else if(Slots.HotBarSlotsRect.Contains(Pos)) {
			return true;
		} else if(Slots.InventorySlotsRect.Contains(Pos)) {
			return true;
		} else {
			return false;
		}
		
	}
	
	public void InitialLoad() {
		
		//Part where the Items are charged
		if(!itemsLoaded) {
			
			ItemBase.MainBase = new InventoryItem[MaxItemBase];
			ItemBase.MainBase = Load();
			Slots.HotBarSlots = new InventoryItem[LvlSys.HTS];
			Slots.InventorySlots = new InventoryItem[LvlSys.IIX*LvlSys.IIY];
			Slots.AmmoSlots = new InventoryItem[LvlSys.Ammo];
			Slots.Accesories = new InventoryItem[LvlSys.AccsX*LvlSys.AccsY];
			SelectedSlot.SSlot = 0;
			//Slots.EquipedItem = new InventoryItem[Set_Ammo(lvl)+Set_AccsX(lvl)*Set_AccsY(lvl)+Armor];
			itemsLoaded = true;
			Slots.AccesoriesRect = new Rect(Screen.width/2-(43*LvlSys.AccsX/2)+235, Screen.height/2-240+20+50, LvlSys.AccsX*43, LvlSys.AccsY*43);
			Slots.AmmoSlotsRect = new Rect(Screen.width/2-(43*LvlSys.Ammo/2)+235, Screen.height/2-130+20+50, LvlSys.Ammo*43, 43);
			Slots.InventorySlotsRect = new Rect(Screen.width/2-(LvlSys.IIX*43/2), Screen.height/2+20+20, LvlSys.IIX*43, LvlSys.IIY*43);
			Slots.HotBarSlotsRect = new Rect(Screen.width/2-(43*LvlSys.HTS/2), Screen.height/2+220, 43*LvlSys.HTS, 43);
			
			//Add starting kit items
			
			/*AddItem(FindItem(64));
			AddItem(FindItem(65));
			AddItem(FindItem(66));
			AddItem(FindItem(128));
			AddItem(FindItem(140));*/
			
		}
		//End that part
		
	}
	
	public void Draw() {
		
		//Not changeable things
		int Armor = 4;
		int Clothes = 0; //Not implemented yet
		int Dyes = 0; //Not implemented yet
		
		GUIStyle lblStyle = new GUIStyle();
		
		lblStyle.fontStyle = FontStyle.Bold;
		lblStyle.normal.textColor = Color.white;
		lblStyle.fontSize = 12;
		lblStyle.alignment = TextAnchor.MiddleCenter;
		
		int slot = 0;
		
		if(!itemsLoaded) {
			InitialLoad();
		}
		
		//AddItem(FindItem(itemsBase, "Ring2"));
		
		GUI.Label(new Rect(Screen.width/2-(43*LvlSys.AccsX/2)+235, Screen.height/2-240+50, LvlSys.AccsX*43, 20), "Accesorios", lblStyle);
		
		GUI.Label(new Rect(Screen.width/2-350+275, Screen.height/2-240+50, 100, 20), "Previsualización", lblStyle);
		
		GUI.Label(new Rect(Screen.width/2-(43*LvlSys.Ammo/2)+235, Screen.height/2-130+50, LvlSys.Ammo*43, 20), "Munición", lblStyle);
		
		GUI.Label(new Rect(Screen.width/2-50, Screen.height/2+20, 100, 20), "Inventario", lblStyle);
		
		//Part where the items are set
		
		if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Mouse1) || Input.GetKeyDown(KeyCode.Mouse2)) {
			mouseheld = false;
		}
		if (Input.GetKeyUp(KeyCode.Mouse0) || Input.GetKeyUp(KeyCode.Mouse1) || Input.GetKeyUp(KeyCode.Mouse2)) {
			mouseheld = true;
		}
		
		if (mouseheld){
			mouseOver = GUI.tooltip; 
		}
		//Debug.Log (GUI.tooltip);
		
		Vector2 mousePosFix = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);
		
		if(mouseitem != null){
			//Debug.Log("Trying to drop");
			if (!IsInventory(mousePosFix)) { //(mouseOver == ""){//still need to add tooltip to other smaller things muh, this will also work for looking around
				//Debug.Log("Trying to drop1");
				if(Input.GetKeyDown(KeyCode.Mouse0)){
					//Debug.Log("Trying to drop2");
					readytodrop = true;
				}
				if(Input.GetKeyUp(KeyCode.Mouse0) && readytodrop){
					//Debug.Log("Trying to drop3");
					if(mouseitem.droppable){
						DropItem();
						//Debug.Log("Dropped");
						//CHECK TO SEE IF ITS A NO DROP IF SO DELETE IT
						mouseitem = null;// need to add like a drop dialog box here to accept or decline
						readytodrop = false;
					} else {
						mouseitem = null;// need to add like a drop dialog box here to accept or decline
						readytodrop = false;
						//mouseheld = false;
					}
				}
				//GUI.Box(new Rect(0,0,64,32),"Droppable");
			}
			
		}
		
		for (int h = 0; h < LvlSys.HTS; h++) {
			inventoryItemSlot(Slots.HotBarSlots, h, Screen.width/2-(43*LvlSys.HTS/2)+(43*h), Screen.height/2+220);
			//ItemBox(Screen.width/2-(43*HTS/2)+(43*h), Screen.height/2+220);
		}
		
		for (int am = 0; am < LvlSys.Ammo; am++) {
			inventoryItemSlot(Slots.AmmoSlots, am, Screen.width/2-(43*LvlSys.Ammo/2)+(43*am)+235, Screen.height/2-110+50);
		}
		
		slot = 0; //Set_Ammo(lvl);
		
		for (int ax = 0; ax < LvlSys.AccsX; ax++) {
			for (int ay = 0; ay < LvlSys.AccsY; ay++) {
				inventoryItemSlot(Slots.Accesories, slot, Screen.width/2-(43*LvlSys.AccsX/2)+(43*ax)+235, Screen.height/2+(ay*43)-220+50);
				slot++;
			}
		}
		
		slot = 0;
		
		for (int iy = 0; iy < LvlSys.IIY; iy++) {
			for (int ix = 0; ix < LvlSys.IIX; ix++) {
				inventoryItemSlot(Slots.InventorySlots, slot, Screen.width/2-350+6+(43*ix), Screen.height/2+40+(43*iy));
				slot++;
			}
		}
		
		//Load saved inventory
		
		if(!gameInvLoaded) {
			//string lvlName = INI_Manager.Load_Value("lastWorldPlayed", Application.dataPath + "/appConfig.cfg");
			PlayerSystem.LoadInventory(Profile.lastPlayedWorld);
			gameInvLoaded = true;
		}
		
		//Part where the Items are drawed
		
		if (mouseitem != null){
			Screen.showCursor = false;
			GUI.DrawTexture(new Rect(Input.mousePosition.x, Screen.height - Input.mousePosition.y,iconSizeW, iconSizeH), mouseitem.itemtex);
		} else {
			Screen.showCursor = true;
		}
		
		//End that part
		
	}
	
	private InventoryItem[] SetItem(InventoryItem[] arraypassed, int slot, InventoryItem item) {
		
		arraypassed[slot] = item;
		
		return arraypassed;
		
	}
	
	private bool ItemBox(float x, float y, string str = "", string guic = "") {
		
		GUIStyle boxstyle = new GUIStyle ();
		
		//Fondo y textura
		Texture2D boxfondo = (Texture2D)Resources.Load ("images/ItemBox3");
		boxstyle.normal.background = boxfondo;
		
		//Font
		boxstyle.fontSize = 12;
		boxstyle.fontStyle = FontStyle.Bold;
		boxstyle.normal.textColor = Color.white;
		boxstyle.alignment = TextAnchor.MiddleCenter;
		
		if (GUI.Button (new Rect (x, y, 39, 41), new GUIContent(str, guic), boxstyle)) {
			return true;		
		}
		
		return false;
		
	}
	
	
}
