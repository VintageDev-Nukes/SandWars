using UnityEngine;
using System;
using System.Collections;

public class Jobs {

	//Jobs: Farmer, Scientist, Doctor, Chef, Engineer, Boxer, Soldier, Police Officer, Fireman, 
	//Athlete, Thief (tienes mayor probabilidad de encontrar buenos items), 
	//Pirate (you earn money easily (+30% bonus money)), Survivor (tienes una mayor resistencia y mayor vida)
	//Hunter (tienes mas resistencia a la inanicion (sed y hambre) (solo survival mode) + mas practica con las armas), 
	//Student (ganas experiencia con mas facilidad y empiezas con bonus)
	//Fisherman (puedes producir peces diariamente (?)), Mago (puedes tener armas magicas)
	//Arquero (puedes llevar arcos y armas medievales (como espadas))
	//+ Roleplaying jobs, (Bukkit jobs + Dead Frontier)
	
	//http://deadfrontier.wikia.com/wiki/Profession
	//https://github.com/phrstbrn/Jobs/blob/master/sample-config/jobConfig.yml
	
	public static readonly string[] strJobs = new string[] {"Agricultor", "Científico", "Doctor"};
	public static readonly string[] JobsDesc = new string[] {"- Puedes crear comidas de alta calidad."+Environment.NewLine+"- Comienzas con...", 
		"- Puedes crear antibióticos de alta calidad."+Environment.NewLine+"- Comienzas con $50 extra.",
		"- Puede administrar tratamientos."+Environment.NewLine+"- Comienzas con $80 extra."};
	public static readonly int[,] JobsBonus = new int[,] {{0, 0, 0, 0}, {0, 0, 0, 0}, {0, 0, 0, 0}};

}
