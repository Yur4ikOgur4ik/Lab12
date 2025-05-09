﻿using MusicalInstruments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collections
{
    public class MyOpenHs<T> where T : MusicalInstrument, IInit, ICloneable
    {
        public HashPoint<T>[] set; //начало списка
        int count = 0; //кол-во элементов
        double LoadFactor; //коэффициент заполненности

        public int Count => count;
        public bool IsReadOnly => throw new NotImplementedException();

        public MyOpenHs(int capacity = 10, double loadfactor = 0.72)
        {
            if (capacity < 0)
                throw new Exception("Capacity can't be <0");
            if (loadfactor < 0 || loadfactor > 1)
                throw new Exception("Koef zapolnennosti must be between 0 and 1");
            set = new HashPoint<T>[capacity]; // создали массив элементов
            LoadFactor = loadfactor;
        }
        public void Add(T itemToAdd)
        {
            T item = (T)(object)itemToAdd;
            if (item == null)
                throw new Exception("Element is empty");
            if (count >= LoadFactor * set.Length)
                Resize(); //Увеличить размер в два раза
            int index = Math.Abs(item.GetHashCode()) % set.Length;
            if (set[index] == null || set[index].IsDeleted)
            {
                set[index] = new HashPoint<T>(item);
                count++;
            }
            else//ищем место
            {
                for (int i = 0; i < set.Length; i++)
                {
                    int newIndex = (index + i) % set.Length; //новый индекс
                    if (set[newIndex] != null &&
                        set[newIndex].Data.Equals(item)) //есть такой элемент
                        return;
                    if (set[newIndex] == null || set[newIndex].IsDeleted)
                    {
                        set[newIndex] = new HashPoint<T>(item);
                        count++;
                        return;
                    }
                }
            }
        }

        public string PrintHS()
        {
            string result = "";

            for (int i = 0; i < set.Length; i++)
            {
                // Формируем строку для текущей ячейки
                if (set[i] == null || set[i].IsDeleted)
                {
                    result += $"{i} : empty\n";
                }
                else
                {
                    result += $"{i} : {set[i].Data}\n";
                }
            }

            return result;
        }

        public void Clear()
        {
            for (int i = 0; i < set.Length; i++)
                set[i] = null;
            count = 0;
        }

        public void Resize()
        {
            HashPoint<T>[] newSet = new HashPoint<T>[set.Length * 2]; // увеличиваем размер в 2 раза

            for (int i = 0; i < set.Length; i++)
            {
                HashPoint<T> item = set[i];
                if (item == null || item.IsDeleted) continue; // пустой или отмечен как удаленный => пропускаем

                int index = Math.Abs(item.GetHashCode()) % newSet.Length;

                if (newSet[index] == null)
                {
                    newSet[index] = item;
                }
                else // ищем место
                {
                    for (int j = 0; j < newSet.Length; j++)
                    {
                        int newIndex = (index + j) % newSet.Length; // новый индекс
                        if (newSet[newIndex] == null)
                        {
                            newSet[newIndex] = item;
                            break;
                        }
                    }
                }
            }

            set = newSet;
        }



        public bool Contains(T item)
        {
            if (item == null) return false;

            int index = Math.Abs(item.GetHashCode()) % set.Length;

            for (int i = 0; i < set.Length; i++)
            {
                int currentIndex = (index + i) % set.Length;
                if (set[currentIndex] == null) return false;
                if (!set[currentIndex].IsDeleted && set[currentIndex].Data.Equals(item))
                    return true;
            }

            return false;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(MusicalInstrument itemToDelete)
        {
            T item = (T)(object)itemToDelete;
            if (item == null) return false;//не удалили
            int index = Math.Abs(item.GetHashCode()) % set.Length;//нидекс в массиве
            if (set[index] != null)//элемент может быть непустой или пустой (null)
            {
                if (!set[index].IsDeleted && set[index].Data.Equals(item)) //элемент не удален и равен item
                {
                    count--;
                    set[index].IsDeleted = true;
                    return true;
                }
                else //нужен проход по массиву
                {
                    for (int i = 0; i < set.Length; i++)
                    {
                        index = (index + i) % set.Length;//след. индекс
                        if (set[index] == null) return false;//ничего не было
                        else
                        {
                            if (!set[index].IsDeleted && set[index].Data.Equals(item)) //элемент не удален и равен item
                            {
                                count--;
                                set[index].IsDeleted = true;
                                return true;
                            }
                        }
                    }
                }
            }
            else
                return false;//если пустой, то такого элемента не было
            return false;

        }
        public T Find(MusicalInstrument itemToFind, out int indexCount)
        {
            indexCount = 0;
            T item = (T)(object)itemToFind;

            if (item == null) return default;

            int index = Math.Abs(item.GetHashCode()) % set.Length;

            for (int i = 0; i < set.Length; i++)
            {
                int currentIndex = (index + i) % set.Length;
                if (set[currentIndex] == null) return default; // конец цепочки пробирования — элемент не найден
                if (!set[currentIndex].IsDeleted && set[currentIndex].Data.Equals(item))
                {
                    indexCount = currentIndex;
                    return set[currentIndex].Data; // нашли совпадение
                }
                    
                
            }

            return default; // элемент не найден
        }
    }
}
