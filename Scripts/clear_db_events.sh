#!/bin/bash

psql -U postgres -h localhost -d tbsprpg_game -c "DELETE FROM games"
psql -U postgres -h localhost -d tbsprpg_game -c "DELETE FROM processed_events"
psql -U postgres -h localhost -d tbsprpg_game -c "DELETE FROM event_type_positions"
psql -U postgres -h localhost -d tbsprpg_game -c "DELETE FROM contents"

psql -U postgres -h localhost -d tbsprpg_map -c "DELETE FROM games"
psql -U postgres -h localhost -d tbsprpg_map -c "DELETE FROM locations"
psql -U postgres -h localhost -d tbsprpg_map -c "DELETE FROM routes"
psql -U postgres -h localhost -d tbsprpg_map -c "DELETE FROM processed_events"
psql -U postgres -h localhost -d tbsprpg_map -c "DELETE FROM event_type_positions"

psql -U postgres -h localhost -d tbsprpg_gamesystem -c "DELETE FROM processed_events"
psql -U postgres -h localhost -d tbsprpg_gamesystem -c "DELETE FROM event_type_positions"

#psql -U postgres -h localhost -d tbsprpg_content -c "DELETE FROM games"
#psql -U postgres -h localhost -d tbsprpg_content -c "DELETE FROM event_type_positions"
#psql -U postgres -h localhost -d tbsprpg_content -c "DELETE FROM processed_events"
