using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellController : MonoBehaviour {

	public float maxMana = 5.0f;
	public float manaRechargeTime = 5.0f;

	public Image manaUI;
	public Image spellIcon;
	public Transform spellOrigin;
	public List<Spell> spells;

	private int activeSpell;

	private float currentMana;
	private float manaPerSec;

	private void Start() {
		manaPerSec = (maxMana / manaRechargeTime);
		currentMana = maxMana;
	}

	private void Update() {
		if(Input.GetKeyDown(KeyCode.E))
			NextSpell();

		currentMana += manaPerSec * Time.deltaTime;
		currentMana = Mathf.Clamp(currentMana, 0f, maxMana);
		if(manaUI != null)
			manaUI.fillAmount = (currentMana / maxMana);
	}

	public void UseActiveSpell(Vector3 _dir, Entity _target) {
		if(!CanUseActiveSpell())
			return;

		Spell usedSpell = spells[this.activeSpell];
		currentMana -= usedSpell.manaRequired;

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

		UpdateSpellIcon();
	}

	public void SetSpell(int _spell) {
		if(_spell < 0 || _spell >= spells.Count)
			return;

		activeSpell = _spell;
		UpdateSpellIcon();
	}

	public bool CanUseActiveSpell() {
		Spell usedSpell = spells[this.activeSpell];
		return (currentMana >= usedSpell.manaRequired);
	}

	private void UpdateSpellIcon() {
		if(spellIcon != null)
			spellIcon.sprite = spells[activeSpell].icon;
	}

}
