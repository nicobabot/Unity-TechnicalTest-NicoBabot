using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tangelo.Test.Components
{

    public class SpawnerComponent_Two : MonoBehaviour, ISpawnerComponent
    {
        //I decided to use array insted of list because we are going to iterate a lot the array
        //and then an array would be a better option in my opinion.
        GameObject[] spawn_pool;
        GameObject[] temporal_list;
        int number_of_object_spawn = 0;

        public Text Ui_spawned;
        public Text Ui_in_pool;

        public Sprite sprite_to_spawn;

        SpriteRenderer sprite_renderer;

        public float scale_sprites_public = 50.0f;
        Vector3 scale_sprites;

        // QUESTION 3: We want to implement a Spawn system using a Pool. Objects should be recycled
        // Re-implement this method to use a Pool instead of creation on-demand.

        public void PopulatePool(int number_of_objects, Slider ui_slider)
        {
            number_of_object_spawn = number_of_objects;

            //To only store memory at the start of the game
            scale_sprites = new Vector3(scale_sprites_public, scale_sprites_public, scale_sprites_public);

            //We will save memory for two lists, the list of the pool and a temporal list
            //the temporal is for when we need more memory in the pool list, then we will need to save
            //the pool info in other list while we are saving memory for the pool list
            spawn_pool = new GameObject[number_of_object_spawn];
            temporal_list = new GameObject[number_of_object_spawn];

            //Then we populate the pool list with gameobjects not actives objects
            for (int i = 0; i < number_of_object_spawn; i++)
            {
                GameObject temporal_gameobject = new GameObject();
                temporal_gameobject.transform.SetParent(transform);
                //We add the sprite renderer to use sprite 2D
                sprite_renderer = temporal_gameobject.AddComponent<SpriteRenderer>();
                sprite_renderer.sprite = sprite_to_spawn;
                temporal_gameobject.SetActive(false);
                spawn_pool[i] = temporal_gameobject;
                ui_slider.value = i;
            }
        }

        public void Spawn(Vector3 position, int number_object_total_spawn)
        {
            //This bool is for we need more memory in the pool list and we have to save more memory
            bool need_more_memory = true;

            //For not spending a lot of iterations if we find a gameobject not active will finish the iteration
            //and we will use it to spawn it
            for (int i = 0; i < spawn_pool.Length; i++)
            {
                if (spawn_pool[i].activeSelf == false)
                {
                    need_more_memory = false;
                    GameObject temporal_gameobject = spawn_pool[i];
                    temporal_gameobject.transform.position = position;
                    temporal_gameobject.transform.localScale = scale_sprites;
                    temporal_gameobject.transform.rotation = Quaternion.identity;
                    temporal_gameobject.SetActive(true);

                    if(temporal_gameobject.GetComponent<SpinnerComponent>() == null)
                    {
                        temporal_gameobject.AddComponent<SpinnerComponent>();
                    }

                    spawn_pool[i] = temporal_gameobject;
                    break;
                }
            }

            //If we didn't find any object not active and we want to spawn we will save more memory
            if (need_more_memory == true)
            {
                //We save the information of the pool list in a temporal
                temporal_list = spawn_pool;

                //We add more memory to the pool list
                spawn_pool = new GameObject[temporal_list.Length + number_of_object_spawn];

                //Then we copy all the info to the pool list
                for (int i = 0; i < temporal_list.Length; i++)
                {
                    spawn_pool[i] = temporal_list[i];
                }

                //--------------------------------------------------------
                //We create the object we couldn't because of the memory of the pool
                GameObject temporal_gameobject = new GameObject();
                temporal_gameobject.transform.SetParent(transform);
                //We add the sprite renderer to use sprite 2D
                sprite_renderer = temporal_gameobject.AddComponent<SpriteRenderer>();
                sprite_renderer.sprite = sprite_to_spawn;
                temporal_gameobject.transform.position = position;
                temporal_gameobject.transform.localScale = scale_sprites;
                temporal_gameobject.transform.rotation = Quaternion.identity;
                temporal_gameobject.SetActive(true);
                if (temporal_gameobject.GetComponent<SpinnerComponent>() == null)
                {
                    temporal_gameobject.AddComponent<SpinnerComponent>();
                }
                spawn_pool[temporal_list.Length] = temporal_gameobject;
                //-------------------------------------------------------

                //and we add the other gameobjects not actives
                for (int i = temporal_list.Length + 1; i < spawn_pool.Length; i++)
                {
                    GameObject temporal_gameobject_to_pool = new GameObject();
                    temporal_gameobject_to_pool.transform.SetParent(transform);
                    sprite_renderer = temporal_gameobject_to_pool.AddComponent<SpriteRenderer>();
                    sprite_renderer.sprite = sprite_to_spawn;
                    temporal_gameobject_to_pool.SetActive(false);
                    spawn_pool[i] = temporal_gameobject_to_pool;
                }

            }

        }

        // QUESTION 5: Implement here the recycling of the object. Once you called this method, the object should be added to Pool.
        // If the object is in the pool, it should be invisible
        public void Recycle(int number_of_objects)
        {
            int recycled_objects = 0;
            int pool_iterator = 0;

            //We search for gameobjects actives to disable
            while (recycled_objects < number_of_objects && pool_iterator < spawn_pool.Length)
            {

                if (spawn_pool[pool_iterator].activeSelf == true)
                {
                    spawn_pool[pool_iterator].transform.rotation = Quaternion.identity;
                    spawn_pool[pool_iterator].SetActive(false);
                    recycled_objects++;
                }
                pool_iterator++;
            }

            NotifyPoolStatus();
        }

        // QUESTION 9: We want to notify how many objects are spawned and the number of items in the pool.
        // SendMessage or Static Methods to notify the status to the GameManager, is not allowed.
        public void NotifyPoolStatus()
        {

            int spawned_objects = 0;
            int in_pool_objects = 0;

            for (int i = 0; i < spawn_pool.Length; i++)
            {
                if (spawn_pool[i].activeSelf)
                {
                    spawned_objects++;
                }
                else
                {
                    in_pool_objects++;
                }
            }

            Ui_spawned.text = spawned_objects.ToString();
            Ui_in_pool.text = in_pool_objects.ToString();
        }
    }
}