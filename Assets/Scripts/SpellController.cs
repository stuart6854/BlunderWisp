using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellController : MonoBehaviour {

	//TODO: Stamina type cooldown

	public List<Spell> spells;
	public Transform spellOrigin;

	private int activeSpell;

	private void Update() {
		if(Input.GetKeyDown(KeyCode.E))
			NextSpell();

		foreach(Spell spell in spells) {
			if(spell.currentCooldown > 0)
				spell.currentCooldown -= Time.deltaTime;
		}
	}

	public void UseActiveSpell(Vector3 _dir, Entity _target) {
		if(!CanUseActiveSpell())
			return;

		Spell usedSpell = spells[this.activeSpell];
		usedSpell.currentCooldown = usedSpell.cooldownTime;

		GameObject spellObj = Instantiate(usedSpell.gameObject, spellOrigin.position, Quaternion.identity);
		//Rotation
		float angleDeg = Mathf.Atan2(_dir.y, _dir.x) * Mathf.Rad2Deg;
		spellObj.transform.rotation = Quaternion.Euler(0, 0, angleDeg);

		Spell spell = spellObj.GetComponent<Spell>();
		if(_target != null)
			spell.target = _target.transform;
	}

	public void NextSpell() {
		activeSpell++;
		if(activeSpell >= spells.Count)
			activeSpell = 0;
	}

	public void SetSpell(int _spell) {
		if(_spell < 0 || _spell >= spells.Count)
			return;

		activeSpell = _spell;
	}

	public bool CanUseActiveSpell() {
		Spell usedSpell = spells[this.activeSpell];
		return (usedSpell.currentCooldown <= 0f);
	}

}
