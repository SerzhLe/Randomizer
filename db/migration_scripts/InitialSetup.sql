CREATE TABLE IF NOT EXISTS game_config (
	game_config_id uuid PRIMARY KEY,
	display_id SERIAL,
	count_of_rounds INT CHECK(count_of_rounds > 0) NOT NULL
);

CREATE TABLE IF NOT EXISTS participant (
	participant_id uuid PRIMARY KEY,
	nick_name varchar(100),
	position INT CHECK(position >= 0) NOT NULL,
	game_config_id uuid NOT NULL,
	CONSTRAINT participant_game_config_id_fkey 
	FOREIGN KEY (game_config_id) REFERENCES game_config(game_config_id)
);

CREATE TABLE IF NOT EXISTS message (
	message_id uuid PRIMARY KEY,
	content varchar(500) NOT NULL,
	position INT CHECK(position >= 0) NOT NULL,
	game_config_id uuid NOT NULL,
	CONSTRAINT message_game_config_id_fkey 
	FOREIGN KEY (game_config_id) REFERENCES game_config(game_config_id)
);

CREATE TABLE IF NOT EXISTS game_config_round (
	game_config_round_id uuid PRIMARY KEY,
	is_completed BOOLEAN NOT NULL DEFAULT false,
	is_current BOOLEAN NOT NULL DEFAULT false,
	sequence_number INT CHECK(sequence_number >= 0) NOT NULL,
	game_config_id uuid NOT NULL,
	CONSTRAINT game_config_round_game_config_id_fkey 
	FOREIGN KEY (game_config_id) REFERENCES game_config(game_config_id)
);

CREATE TABLE IF NOT EXISTS game_config_round_result (
	game_config_round_result_id uuid PRIMARY KEY,
	score float8 NULL,
	comment varchar(200) NULL,
	who_perform_action_id uuid NOT NULL,
	who_perform_feedback_id uuid NOT NULL,
	message_id uuid NOT NULL,
	game_config_round_id uuid NOT NULL,
	CONSTRAINT game_config_round_result_who_perform_action_id_fkey 
	FOREIGN KEY (who_perform_action_id) REFERENCES participant(participant_id),
	CONSTRAINT game_config_round_result_who_perform_feedback_id_fkey 
	FOREIGN KEY (who_perform_feedback_id) REFERENCES participant(participant_id),
	CONSTRAINT game_config_round_result_message_id_fkey 
	FOREIGN KEY (message_id) REFERENCES message(message_id),
	CONSTRAINT game_config_round_result_game_config_round_id_fkey 
	FOREIGN KEY (game_config_round_id) REFERENCES game_config_round(game_config_round_id)
);