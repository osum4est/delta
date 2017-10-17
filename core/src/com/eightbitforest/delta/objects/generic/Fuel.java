package com.eightbitforest.delta.objects.generic;

import com.eightbitforest.delta.level.Level;
import com.eightbitforest.delta.objects.powerups.Powerup;
import com.eightbitforest.delta.utils.Colors;
import com.eightbitforest.delta.utils.Constants;
import com.eightbitforest.delta.utils.Ids;

public class Fuel extends Powerup {
    public Fuel(Level level, float x, float y) {
        super(level, Ids.FUEL, x, y, Colors.FUEL);
    }

    @Override
    public void activate(Player player) {
        player.setdV(Constants.PLAYER_MAX_DV);
    }
}
