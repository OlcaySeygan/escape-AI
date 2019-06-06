using System;
using System.Collections.Generic;

/// <summary>
/// Neural Network C#
/// </summary>
public class NeuralNetwork : IComparable<NeuralNetwork>
{
	private int[] layers; // Katmanlar.
	private float[][] neurons; // Nöran matris.
	private float[][][] weights; // Ağırlık matrisi.
	private float fitness; //fitness of the network

	/// <summary>
	/// Sinir ağını rasgele ağırlıklarla oluşturur.
	/// </summary>
	/// <param name="layers">Sinir ağı katmanları</param>
	public NeuralNetwork(int[] layers)
	{
		// Bu ağ katmanlarının derin kopyası.
		this.layers = new int[layers.Length];
		for (int i = 0; i < layers.Length; i++)
			this.layers[i] = layers[i];

		// Matris oluştur.
		InitNeurons();
		InitWeights();
	}

	/// <summary>
	/// Derin kopya oluşturucu. 
	/// </summary>
	/// <param name="copyNetwork">Derin ağ kopyası</param>
	public NeuralNetwork(NeuralNetwork copyNetwork)
	{
		this.layers = new int[copyNetwork.layers.Length];
		for (int i = 0; i < copyNetwork.layers.Length; i++)
			this.layers[i] = copyNetwork.layers[i];

		InitNeurons();
		InitWeights();
		CopyWeights(copyNetwork.weights);
	}

	private void CopyWeights(float[][][] copyWeights)
	{
		for (int i = 0; i < weights.Length; i++)
			for (int j = 0; j < weights[i].Length; j++)
				for (int k = 0; k < weights[i][j].Length; k++)
					weights[i][j][k] = copyWeights[i][j][k];
	}

	/// <summary>
	/// Nöral matrisi oluştur.
	/// </summary>
	private void InitNeurons()
	{
		List<float[]> neuronsList = new List<float[]>(); // Nöron hazırlığı.
		for (int i = 0; i < layers.Length; i++) // Tüm katmanlar üzerinde çalış.
			neuronsList.Add(new float[layers[i]]); // Nöron listesine katman ekle.
		neurons = neuronsList.ToArray(); // Listeyi dizine çevir.
	}

	/// <summary>
	/// Ağırlık matrisi oluştur.
	/// </summary>
	private void InitWeights()
	{
		List<float[][]> weightsList = new List<float[][]>(); // Daha sonra 3B dizine ile değiştirilicek olan dizin.
		for (int i = 1; i < layers.Length; i++) // Ağırlık bağlantılı tüm nöronların yinelemesi.
		{
			List<float[]> layerWeightList = new List<float[]>(); // Bu geçerli katman için ayer ağırlık listesi (2B dizisine dönüştürülecektir).
			int neuronsInPreviousLayer = layers[i - 1];
			for (int j = 0; j < neurons[i].Length; j++) // Bu geçerli katmandaki tüm nöronları yinele.
			{
				float[] neuronWeights = new float[neuronsInPreviousLayer]; // Nöron ağırlıkları.

				// 1 ve -1 arasında rasgele bir ağırlık olarak ayarla.
				for (int k = 0; k < neuronsInPreviousLayer; k++)
					neuronWeights [k] = GetRandomNumber (-1f, 1f);
				layerWeightList.Add(neuronWeights);
			}
			weightsList.Add(layerWeightList.ToArray());
		}
		weights = weightsList.ToArray();
	}

	/// <summary>
	/// Belirli bir girdi dizisi ile bu sinir ağını besleme.
	/// </summary>
	/// <param name="inputs">girdiler ağa</param>
	/// <returns></returns>
	public float[] FeedForward(float[] inputs)
	{
		for (int i = 0; i < inputs.Length; i++)        
			neurons[0][i] = inputs[i];


		// i - Katmanlar
		// j - Nöronlar
		// k - Ağırlıklar
		for (int i = 1; i < layers.Length; i++)
			for (int j = 0; j < neurons[i].Length; j++)
			{
				float value = 0f;
				for (int k = 0; k < neurons[i-1].Length; k++)
					value += weights[i - 1][j][k] * neurons[i - 1][k];
				neurons[i][j] = (float)Math.Tanh(value);
			}

		return neurons[neurons.Length - 1];
	}

	/// <summary>
	/// Sinir ağı ağırlıkları mutasyona uğrat.
	/// </summary>
	public void Mutate()
	{
		for (int i = 0; i < weights.Length; i++)
			for (int j = 0; j < weights[i].Length; j++)
				for (int k = 0; k < weights[i][j].Length; k++)
				{
					float weight = weights[i][j][k];

					// Ağırlık değerini mutasyona uğrat.
					float randomNumber = GetRandomNumber(0f, 10f);
					if (randomNumber <= 2f)
					{ //if 1
						//flip sign of weight
						weight *= -1f;
					}
					else if (randomNumber <= 4f)
					{ //if 2
						//pick random weight between -1 and 1
						weight = GetRandomNumber(-0.5f, 0.5f);
					}
					else if (randomNumber <= 6f)
					{ //if 3
						//randomly increase by 0% to 100%
						float factor = GetRandomNumber(0f, 1f) + 1f;
						weight *= factor;
					}
					else if (randomNumber <= 8f)
					{ //if 4
						//randomly decrease by 0% to 100%
						float factor = GetRandomNumber(0f, 1f);
						weight *= factor;
					}

					weights[i][j][k] = weight;
				}
	}

	public void AddFitness(float fit)
	{
		fitness += fit;
	}

	public void SetFitness(float fit)
	{
		fitness = fit;
	}

	public float GetFitness()
	{
		return fitness;
	}

	/// <summary>
	/// İki sinir ağını karşılaştırın ve uygunluğa göre sıralama yapın.
	/// </summary>
	/// <param name="other">karşılaştırılacak ağ</param>
	/// <returns></returns>
	public int CompareTo(NeuralNetwork other)
	{
		if (other == null) return 1;

		if (fitness > other.fitness)
			return 1;
		else if (fitness < other.fitness)
			return -1;
		else
			return 0;
	}

	Random random = new Random();
	private float GetRandomNumber(double minimum, double maximum)
	{
		return (float)(random.NextDouble() * (maximum - minimum) + minimum);
	}

	private float sigmoid(float value){
		return (float)(1f / (1f + Math.Pow(Math.E, -value)));
	}
}