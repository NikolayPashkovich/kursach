using System.Collections.Generic;
using UnityEngine;

public static class RandomSelector
{
    public static T GetRandomObjectInverseWeigths<T>(List<T> objects, List<int> weights)
    {
        // Сначала мы находим общий вес, который будет суммой обратных значений весов
        float totalInverseWeight = 0f;
        for (int i = 0; i < weights.Count; i++)
        {
            totalInverseWeight += invetseWeigth(weights[i]);
        }

        // Генерируем случайное число от 0 до суммы обратных весов
        float randomValue = Random.Range(0f, totalInverseWeight);

        // Теперь идем по объектам и суммируем обратные веса, пока не найдем нужный объект
        float cumulativeWeight = 0f;
        for (int i = 0; i < objects.Count; i++)
        {
            cumulativeWeight += invetseWeigth( weights[i]);
            if (randomValue < cumulativeWeight)
            {
                return objects[i];
            }
        }

        // Если по какой-то причине не сработало (например, округление), возвращаем последний объект
        return objects[objects.Count - 1];
    }
    public static int GetRandomIndexInverseWeigths(int[] weights)
    {
        // Сначала мы находим общий вес, который будет суммой обратных значений весов
        float totalInverseWeight = 0f;
        for (int i = 0; i < weights.Length; i++)
        {
            totalInverseWeight += invetseWeigth(weights[i]);
        }

        // Генерируем случайное число от 0 до суммы обратных весов
        float randomValue = Random.Range(0f, totalInverseWeight);

        // Теперь идем по объектам и суммируем обратные веса, пока не найдем нужный объект
        float cumulativeWeight = 0f;
        for (int i = 0; i < weights.Length; i++)
        {
            cumulativeWeight += invetseWeigth(weights[i]);
            if (randomValue < cumulativeWeight)
            {
                return i;
            }
        }

        // Если по какой-то причине не сработало (например, округление), возвращаем последний объект
        return weights.Length - 1;
    }
    static float invetseWeigth(float weigth)
    {
        return (1f / (weigth + 1));
    }
    static int Weigth(int weigth)
    {
        return weigth + 1;
    }
}
