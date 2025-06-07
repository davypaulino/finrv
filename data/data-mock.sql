INSERT INTO ativo (id, codigo, nome, criado_em, criado_por) VALUES
(1, 'ITUB4', 'Itaú Unibanco PN', NOW(),'test'),
(2, 'ITUB3', 'Itaú Unibanco ON', NOW(), 'test'),
(3, 'B3SA3', 'B3 - Bolsa de Valores', NOW(), 'test');

INSERT INTO usuario (id, nome, email, porcentagem_corretagem, criado_em, criado_por) VALUES
('57e8b488-f85c-42de-a1b4-03e8bc0c58a2', 'Carlos Silva', 'carlos.silva@example.com', 1.50, NOW(), 'test'),
('17a0ad41-eca4-478e-9dd5-f8b2b218e848', 'Ana Souza', 'ana.souza@example.com', 2.00, NOW(), 'test');

INSERT INTO cotacao (ativo_id, preco_unitario, criado_em, criado_por) VALUES
(1, 37.99, NOW(), 'test'),
(2, 32.33, NOW(), 'test'),
(3, 12.26, NOW(), 'test');

INSERT INTO operacoes (usuario_id, ativo_id, quantidade, preco_unitario, tipo_operacao, corretagem, criado_em, criado_por) VALUES
('57e8b488-f85c-42de-a1b4-03e8bc0c58a2', 1, 25, 37.99, 1, 1.50, '2025-06-05 09:15:32', 'test'),
('57e8b488-f85c-42de-a1b4-03e8bc0c58a2', 1, 13, 37.99, 1, 1.25, '2025-06-05 10:30:20', 'test'),
('57e8b488-f85c-42de-a1b4-03e8bc0c58a2', 2, 24, 32.33, 1, 1.50, '2025-06-05 11:45:10', 'test'),
('57e8b488-f85c-42de-a1b4-03e8bc0c58a2', 3, 3, 12.26, 1, 1.57, '2025-06-05 12:55:45', 'test'),
('57e8b488-f85c-42de-a1b4-03e8bc0c58a2', 3, 2, 12.26, 2, 1.57, '2025-06-05 14:05:15', 'test'),
('17a0ad41-eca4-478e-9dd5-f8b2b218e848', 3, 20, 12.26, 1, 1.57, '2025-06-05 15:20:50', 'test'),
('17a0ad41-eca4-478e-9dd5-f8b2b218e848', 2, 50, 32.33, 1, 1.50, '2025-06-05 16:40:38', 'test'),
('17a0ad41-eca4-478e-9dd5-f8b2b218e848', 2, 11, 32.33, 2, 1.50, '2025-06-05 18:05:22', 'test');

INSERT INTO posicao (id, usuario_id, ativo_id, quantidade, preco_medio, lucro_ou_perda, criado_em, criado_por) VALUES
(UUID(), '57e8b488-f85c-42de-a1b4-03e8bc0c58a2', 1, 38, 37.99, 0.00, NOW(), 'test'),
(UUID(), '57e8b488-f85c-42de-a1b4-03e8bc0c58a2', 2, 13, 32.33, 0.00, NOW(), 'test'),  -- Posição consolidada de ITUB3 (24 compras - 11 vendas)
(UUID(), '57e8b488-f85c-42de-a1b4-03e8bc0c58a2', 3, 1, 12.26, 0.00, NOW(), 'test'),    -- Posição consolidada de B3SA3 (3 compras - 2 vendas)
(UUID(), '17a0ad41-eca4-478e-9dd5-f8b2b218e848', 3, 20, 12.26, 0.00, NOW(), 'test'),  -- Posição consolidada de B3SA3 para outro usuário
(UUID(), '17a0ad41-eca4-478e-9dd5-f8b2b218e848', 2, 39, 32.33, 0.00, NOW(), 'test'); -- Posição consolidada de ITUB3 (50 compras - 11 vendas)

SELECT  * from cotacao;