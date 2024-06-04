﻿using System.Collections.ObjectModel;

namespace Amz.Library;

public class InventoryServiceProxy
{
    private InventoryServiceProxy(){ 
        items = new List<Item>();
    }
    private static InventoryServiceProxy? instance;
    private static object instanceLock = new object();
    public static InventoryServiceProxy Current{
        get{
            
            lock(instanceLock) //this makes it thread safe, we need singletons to be thread safe
            {
                if(instance == null){
                    instance = new InventoryServiceProxy();
                }
            }
            
            
            return instance;
        }
    }

    private List<Item>? items;
    public ReadOnlyCollection<Item>? Items { //this is the read
        get
        {
            return items?.AsReadOnly();
        }
    }

    //================functionality================
    public int LastId{
        get{
            if(items?.Any() ?? false){
                return items?.Select(c => c.Id)?.Max() ?? 0;
            }
            return 0;
        }
    }
    public Item? invAdd(Item item)
    {
        if(items == null)
        {
            return null;
        }
        var isAdd = false;

        if(item.Id == 0){
            item.Id = LastId + 1;
            isAdd = true;
        }
        if(isAdd){
            items.Add(item);
        }
        return item;
    }
    public Item? FindById(int id){
        return items?.FirstOrDefault(c => c.Id == id);
    }
    
    public void Delete(int id){
        if(items == null){
            return;
        }
        
        var itemToDelete = items.FirstOrDefault(c => c.Id == id);

        if(itemToDelete != null){
            items.Remove(itemToDelete);
        }
    }
}