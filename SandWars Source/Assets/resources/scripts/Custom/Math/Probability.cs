using System;

public class Probability {

	public static bool Prob(float probabilidad = 100) {
		Random random = new Random();
		return random.NextDouble() < (probabilidad/100);
	}

}
